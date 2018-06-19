using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using DocWorks.Integration.XmlDoc.Extensions;

namespace DocWorks.Integration.XmlDoc
{
    internal class XmlDocReplacerVisitor : CSharpSyntaxRewriter
    {
        private SemanticModel _semanticModel;
        private XmlDocument _xmlDoc;
        private PartialTypeInfoCollectorVisitor _partialTypeInfoCollector;

        public XmlDocReplacerVisitor(string docXml, PartialTypeInfoCollectorVisitor partialInfoCollector) : base(true)
        {
            _partialTypeInfoCollector = partialInfoCollector;
            _xmlDoc = new XmlDocument();
            _xmlDoc.LoadXml(docXml);

        }

        internal SyntaxNode Visit(SyntaxNode rootNode, SemanticModel semanticModel)
        {
            _semanticModel = semanticModel;
            return Visit(rootNode);
        }

        public override SyntaxNode Visit(SyntaxNode node)
        {
            var updatedNode = base.Visit(node);
            if (node is BaseTypeDeclarationSyntax baseTypeDeclarationSyntax)
                return AddOrUpdateXmlDoc(baseTypeDeclarationSyntax, (BaseTypeDeclarationSyntax)updatedNode);

            if (node is BaseFieldDeclarationSyntax baseFieldDeclarationSyntax)
                return VisitBaseFieldDeclaration(baseFieldDeclarationSyntax);

            if (node is MemberDeclarationSyntax memberDeclarationSyntax && !(node is NamespaceDeclarationSyntax))
                return AddOrUpdateXmlDoc(memberDeclarationSyntax, (MemberDeclarationSyntax)updatedNode);

            return updatedNode;
        }

        private bool isVisitingField = false;
        private SyntaxNode VisitBaseFieldDeclaration(BaseFieldDeclarationSyntax node)
        {
            //for Field declarations, the Xml may be on the field itself or on the variable declarator.
            // ex. on the field
            //   ///<summary>Docs</summary>
            //   int value;
            // ex. on the variable declarator
            //   int
            //      ///<summary>Docs</summary>
            //      value;
            // ex. with two declarations
            //   int value,
            //      ///<summary>Docs</summary>
            //      value2;

            // We handle this by visiting the Field declaration and the variable declarations, treating them the same way we treat partial classes.
            isVisitingField = true;
            try
            {
                var symbol = _semanticModel.GetDeclaredSymbol(node.Declaration.Variables[0]);
                var docNodes = _xmlDoc.SelectNodes($"descendant::member[@name='{symbol.MemberNameUnescaped()}']/xmldoc");
                var updatedNode = base.Visit(node);
                return AddOrUpdateXmlDoc(node, updatedNode, docNodes, symbol);
            }
            finally
            {
                isVisitingField = false;
            }
        }

        public override SyntaxNode VisitVariableDeclarator(VariableDeclaratorSyntax node)
        {
            if (!isVisitingField)
                return node;

            var typeSymbol = _semanticModel.GetDeclaredSymbol(node);
            Debug.Assert(typeSymbol != null, "No symbol found for field");

            var docNodes = _xmlDoc.SelectNodes($"descendant::member[@name='{typeSymbol.MemberNameUnescaped()}']/xmldoc");

            var updatedNode = AddOrUpdateXmlDoc(node, node, docNodes, typeSymbol);
            return updatedNode;
        }

        private SyntaxNode AddOrUpdateXmlDoc(BaseTypeDeclarationSyntax originalNode, BaseTypeDeclarationSyntax nodeToUpdate)
        {
            var typeSymbol = _semanticModel.GetDeclaredSymbol(originalNode);

            var typeNode = SelectTypeNode(typeSymbol);
            if (typeNode == null)
                return nodeToUpdate;

            var docNode = typeNode["xmldoc"];
            if (docNode == null)
                return nodeToUpdate;

            return AddOrUpdateXmlDoc(originalNode, nodeToUpdate, docNode, typeSymbol);
        }

        private XmlNode SelectTypeNode(INamedTypeSymbol typeSymbol)
        {
            var selector =
                new StringBuilder($"@name='{typeSymbol.MetadataName}' and @namespace='{typeSymbol.ContainingNamespace.FullyQualifiedName(true, true)}'");
            if (typeSymbol.ContainingType != null)
                selector.Append($" and @containingType='{typeSymbol.ContainingType.FullyQualifiedName(false, true)}'");

            var docNodes = _xmlDoc.SelectNodes($"descendant::member[{selector}]");
            if (docNodes.Count > 1)
            {
                throw new DuplicateMemberException($@"Multiple matches for ""{typeSymbol}"" found in xml.\nMatches:\n{string.Join("\n", docNodes.Cast<XmlNode>().Select(n => n.OuterXml))}");
            }

            if (docNodes.Count == 0)
                return null;

            return docNodes[0];
        }

        private SyntaxNode AddOrUpdateXmlDoc(MemberDeclarationSyntax originalNode, MemberDeclarationSyntax nodeToUpdate)
        {
            var symbol = _semanticModel.GetDeclaredSymbol(originalNode);

            XmlNode parentNode;
            if (symbol is INamedTypeSymbol)
                parentNode = _xmlDoc.SelectSingleNode("doc");
            else
                parentNode = SelectTypeNode(symbol.ContainingType);

            if (parentNode == null)
                return nodeToUpdate;

            var constraints = "";
            var methodSymbol = symbol as IMethodSymbol;
            if (methodSymbol != null)
            {
                int xpathIdx = 1;
                foreach (var parameter in methodSymbol.Parameters)
                {
                    var parameterType = parameter.Type;
                    if (parameterType is ITypeParameterSymbol)
                        constraints += $@" and signature/parameters/parameter[{xpathIdx}]/typeParameter/@name='{parameterType.Name}'";
                    else
                        constraints += $@" and signature/parameters/parameter[{xpathIdx}]/type/@typeId='{parameterType.Id()}'";

                    xpathIdx++;
                }
                constraints += $@" and not(signature/parameters/parameter[{xpathIdx}])";
            }

            var docNodes = parentNode.SelectNodes($"member[@name='{symbol.MemberNameUnescaped()}'{constraints}]/xmldoc");

            return AddOrUpdateXmlDoc(originalNode, nodeToUpdate, docNodes, symbol);
        }

        private SyntaxNode AddOrUpdateXmlDoc(SyntaxNode originalNode, SyntaxNode nodeToBeUpdated, XmlNodeList matchingDocNodes, ISymbol symbol)
        {
            if (matchingDocNodes.Count > 1)
            {
                throw new DuplicateMemberException($@"Multiple matches for ""{symbol}"" found in xml.\nMatches:\n{string.Join("\n", matchingDocNodes.Cast<XmlNode>().Select(n => n.OuterXml))}");
            }

            if (matchingDocNodes.Count == 0)
                return nodeToBeUpdated;

            var docNode = matchingDocNodes[0];

            return AddOrUpdateXmlDoc(originalNode, nodeToBeUpdated, docNode, symbol);
        }

        private SyntaxNode AddOrUpdateXmlDoc(SyntaxNode originalNode, SyntaxNode nodeToBeUpdated, XmlNode docNode,
            ISymbol symbol)
        {
            var docTrivia = nodeToBeUpdated.GetLeadingTrivia();

            var initialWhitespace = docTrivia.TakeWhile(t => t.IsKind(SyntaxKind.WhitespaceTrivia) || t.IsKind(SyntaxKind.EndOfLineTrivia)).ToArray();
            int lastNewLineIndex = Array.FindLastIndex(initialWhitespace, t => t.Kind() == SyntaxKind.EndOfLineTrivia);
            if (lastNewLineIndex >= 0)
                initialWhitespace = initialWhitespace.Skip(lastNewLineIndex + 1).ToArray();

            var rawWhitespace = string.Join("", initialWhitespace.Select(t => t.ToFullString()));

            var comment = string.Join("\r\n" + rawWhitespace, docNode.InnerText.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(item => "/// " + item));

            var syntaxTree = CSharpSyntaxTree.ParseText(comment);
            var xmlDocumentNode = syntaxTree.GetRoot();

            var newTrivia = SyntaxFactory.TriviaList();

            var shouldUpdate = _partialTypeInfoCollector.ShouldThisNodeBeDocumented(originalNode, symbol);
            foreach (var trivia in docTrivia)
            {
                if (!trivia.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia))
                {
                    newTrivia = newTrivia.Add(trivia);
                }
                else if (shouldUpdate)
                {
                    shouldUpdate = false;
                    newTrivia = newTrivia.AddRange(xmlDocumentNode.GetLeadingTrivia().Add(SyntaxFactory.CarriageReturnLineFeed).AddRange(initialWhitespace));
                }
            }

            if (shouldUpdate)
            {
                newTrivia = newTrivia.AddRange(xmlDocumentNode.GetLeadingTrivia().Add(SyntaxFactory.CarriageReturnLineFeed).AddRange(initialWhitespace));
            }

            return nodeToBeUpdated.WithLeadingTrivia(newTrivia);
        }
    }
}