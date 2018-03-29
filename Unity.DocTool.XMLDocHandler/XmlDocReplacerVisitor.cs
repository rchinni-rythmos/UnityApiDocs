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

        ///
        /// <summary>teste</summary>
        /// <example>what</example>
        /// 
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

            //var docNode = _xmlDoc.SelectSingleNode($"doc/member[@name='{node.Identifier}' && @namespace='{enumDef.ContainingNamespace}']");
            StringBuilder selector = new StringBuilder($"@name='{typeSymbol.MetadataName}' and @namespace='{typeSymbol.ContainingNamespace}'");
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
            //TODO: check if we can get a null ref here. Does it make sense?
            if (docNode == null) return null;

            var docTrivia = node.GetLeadingTrivia();
            var docT = docTrivia.FirstOrDefault(t => t.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia));

            if (docT.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia))
            {
                // if we are updating the documentation on a node, just remove the existing one.
                node = node.WithoutLeadingTrivia();
            }

            var comment = string.Join("\n",
                docNode.InnerText.Split(new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries)
                    .Where(x => !string.IsNullOrWhiteSpace(x)).Select(item => "/// " + item));
            var syntaxTree = CSharpSyntaxTree.ParseText(comment);
            var xmlDocumentNode = syntaxTree.GetRoot();

            return node.WithLeadingTrivia(xmlDocumentNode.GetLeadingTrivia().Add(SyntaxFactory.LineFeed));
        }

        //public override SyntaxNode VisitDocumentationCommentTrivia(DocumentationCommentTriviaSyntax node)
        //{
        //    var documentationTarget = node.ParentTrivia.Token.Parent;
        //    var typeSymbol = _semanticModel.GetSymbolInfo(documentationTarget);

        //    if (typeSymbol.Symbol.Kind == SymbolKind.NamedType)
        //    {
        //        var typeXmlDocNode = _xmlDoc.SelectSingleNode($"/doc/member[@name='{documentationTarget}'"); //TODO: Check namespace
        //        if (typeXmlDocNode != null)
        //        {
        //        }
        //    }

        //    var t = CSharpSyntaxTree.ParseText(
        //        $"///");

        //    var root = t.GetRoot();
        //    return SyntaxFactory.DocumentationCommentTrivia(
        //                        SyntaxKind.SingleLineDocumentationCommentTrivia).WithLeadingTrivia(root.GetLeadingTrivia());

        //}
    }
}