using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Unity.DocTool.XMLDocHandler.Extensions;

namespace Unity.DocTool.XMLDocHandler
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
                var docNode = _xmlDoc.SelectSingleNode($"descendant::member[@name='{symbol.MemberNameUnescaped()}']/xmldoc");
                var updatedNode = base.Visit(node);
                return AddOrUpdateXmlDoc(node, updatedNode, docNode, symbol);
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

            var docNode = _xmlDoc.SelectSingleNode($"descendant::member[@name='{typeSymbol.MemberNameUnescaped()}']/xmldoc");

            var updatedNode = AddOrUpdateXmlDoc(node, node, docNode, typeSymbol);
            return updatedNode;
        }

        private SyntaxNode AddOrUpdateXmlDoc(BaseTypeDeclarationSyntax originalNode, BaseTypeDeclarationSyntax nodeToUpdate)
        {
            var typeSymbol = _semanticModel.GetDeclaredSymbol(originalNode);

            var typeNode = SelectTypeNode(typeSymbol);
            if (typeNode == null)
                return nodeToUpdate;

            var docNode = typeNode["xmldoc"];
            return AddOrUpdateXmlDoc(originalNode, nodeToUpdate, docNode, typeSymbol);
        }

        private XmlNode SelectTypeNode(INamedTypeSymbol typeSymbol)
        {
            var selector =
                new StringBuilder($"@name='{typeSymbol.MetadataName}' and @namespace='{typeSymbol.ContainingNamespace}'");
            if (typeSymbol.ContainingType != null)
                selector.Append($" and @containingType='{typeSymbol.ContainingType.FullyQualifiedName(false, true)}'");

            var docNode = _xmlDoc.SelectSingleNode($"descendant::member[{selector}]");
            return docNode;
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

            var parameterConstraints = "";
            var methodSymbol = symbol as IMethodSymbol;
            if (methodSymbol != null)
            {
                foreach (var parameter in methodSymbol.Parameters)
                {
                    parameterConstraints += $@" and signature/parameters/parameter/@typeId='{parameter.Type.Id()}'";
                }
            }

            var docNode = parentNode.SelectSingleNode($"member[@name='{symbol.MemberNameUnescaped()}'{parameterConstraints}]/xmldoc");
            return AddOrUpdateXmlDoc(originalNode, nodeToUpdate, docNode, symbol);
        }

        private SyntaxNode AddOrUpdateXmlDoc(SyntaxNode originalNode, SyntaxNode nodeToBeUpdated, XmlNode docNode, ISymbol symbol)
        {
            if (docNode == null) return nodeToBeUpdated;

            var docTrivia = nodeToBeUpdated.GetLeadingTrivia();

            var comment = string.Join("\n", docNode.InnerText.Split(new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries)
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
                    newTrivia = newTrivia.AddRange(xmlDocumentNode.GetLeadingTrivia().Add(SyntaxFactory.LineFeed));
                }
            }

            if (shouldUpdate)
            {
                newTrivia = newTrivia.AddRange(xmlDocumentNode.GetLeadingTrivia().Add(SyntaxFactory.LineFeed));
            }

            return nodeToBeUpdated.WithLeadingTrivia(newTrivia);
        }
    }
}