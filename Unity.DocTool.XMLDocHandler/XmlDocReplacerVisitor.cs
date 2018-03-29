using System;
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

        public XmlDocReplacerVisitor(string docXml) : base(true)
        {
            _xmlDoc = new XmlDocument();
            _xmlDoc.LoadXml(docXml);
        }

        internal SyntaxNode Visit(SyntaxNode rootNode, SemanticModel semanticModel)
        {
            _semanticModel = semanticModel;
            return Visit(rootNode);
        }

        public override SyntaxNode VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            var withLeadingTrivia = AddOrUpdateXmlDoc(node);
            if (withLeadingTrivia != null)
                return withLeadingTrivia;

            return base.VisitInterfaceDeclaration(node);
        }

        public override SyntaxNode VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            var withLeadingTrivia = AddOrUpdateXmlDoc(node);
            if (withLeadingTrivia != null)
                return withLeadingTrivia;

            return base.VisitEnumDeclaration(node);
        }

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            var withLeadingTrivia = AddOrUpdateXmlDoc(node);
            if (withLeadingTrivia != null)
                return withLeadingTrivia;

            return base.VisitClassDeclaration(node);
        }


        public override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            var updatedNode = AddOrUpdateXmlDoc(node);
            if (updatedNode != null)
                return updatedNode;

            return base.VisitPropertyDeclaration(node);
        }

        private SyntaxNode AddOrUpdateXmlDoc(BaseTypeDeclarationSyntax node)
        {
            var typeSymbol = _semanticModel.GetDeclaredSymbol(node);
            if (typeSymbol == null)
                return null;

            var selector = new StringBuilder($"@name='{typeSymbol.MetadataName}' and @namespace='{typeSymbol.ContainingNamespace}'");
            if (typeSymbol.ContainingType != null)
                selector.Append($" and @containingType='{typeSymbol.ContainingType.FullyQualifiedName(false, true)}'");
            
            var docNode = _xmlDoc.SelectSingleNode($"descendant::member[{selector}]/xmldoc");

            return AddOrUpdateXmlDoc(node, docNode);
        }

        private SyntaxNode AddOrUpdateXmlDoc(MemberDeclarationSyntax node)
        {
            var typeSymbol = _semanticModel.GetDeclaredSymbol(node);
            if (typeSymbol == null)
                return null;

            //typeSymbol.ContainingType
            
            //var docNode = _xmlDoc.SelectSingleNode($"doc/member[@name='{node.Identifier}' && @namespace='{enumDef.ContainingNamespace}']");
            var docNode = _xmlDoc.SelectSingleNode($"descendant::member[@name='{typeSymbol.Name}']/xmldoc");

            return AddOrUpdateXmlDoc(node, docNode);
        }

        private static SyntaxNode AddOrUpdateXmlDoc(SyntaxNode node, XmlNode docNode)
        {
            if (docNode == null) return null;

            var docTrivia = node.GetLeadingTrivia();

            var comment = string.Join("\n", docNode.InnerText.Split(new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(item => "/// " + item));

            var syntaxTree = CSharpSyntaxTree.ParseText(comment);
            var xmlDocumentNode = syntaxTree.GetRoot();

            var newTrivia = SyntaxFactory.TriviaList();
            //TODO: Test multiple XML docs in single type/member

            var updated = false;
            foreach (var trivia in docTrivia)
            {
                if (!trivia.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia))
                {
                    newTrivia = newTrivia.Add(trivia);
                }
                else if (updated == false)
                {
                    updated = true;
                    newTrivia = newTrivia.AddRange(xmlDocumentNode.GetLeadingTrivia());
                }
            }

            if (!updated)
            {
                newTrivia = newTrivia.AddRange(xmlDocumentNode.GetLeadingTrivia().Add(SyntaxFactory.LineFeed));
            }

            return node.WithLeadingTrivia(newTrivia);
        }
    }
}