using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Unity.DocTool.XMLDocHandler.Extensions;

namespace Unity.DocTool.XMLDocHandler
{
    public class GetTypesVisitor : CSharpSyntaxWalker
    {
        private List<INamedTypeSymbol> types = new List<INamedTypeSymbol>();
        private SemanticModel semanticModel;

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            if (AddTypeIfPublicAPI(node))
                base.VisitClassDeclaration(node);
        }

        public override void VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            if (AddTypeIfPublicAPI(node))
                base.VisitEnumDeclaration(node);
        }

        public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            if (AddTypeIfPublicAPI(node))
                base.VisitInterfaceDeclaration(node);
        }

        //TODO: Support global delegates

        //public override void VisitDelegateDeclaration(DelegateDeclarationSyntax node)
        //{
        //if (AddTypeIfPublicAPI(node))
        //base.VisitDelegateDeclaration(node);
        //}

        public override void VisitStructDeclaration(StructDeclarationSyntax node)
        {
            if (AddTypeIfPublicAPI(node))
                base.VisitStructDeclaration(node);
        }

        private bool AddTypeIfPublicAPI(BaseTypeDeclarationSyntax node)
        {
            var symbol = semanticModel.GetDeclaredSymbol(node);
            var accessibility = symbol.DeclaredAccessibility;
            if (accessibility == Accessibility.Public ||
                accessibility == Accessibility.Protected ||
                accessibility == Accessibility.ProtectedAndInternal)
            {
                types.Add(symbol);
                return true;
            }

            return false;
        }

        public string GetXml()
        {
            var groups = types.GroupBy(t => FullyQualifiedName(t, true, false));
            StringBuilder output = new StringBuilder();

            output.Append(@"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<doc version=""1"">
    <types>");

            foreach (var group in groups)
            {
                var firstType = group.First();
                output.Append(
                    value: $@"
        <type>
            <id>{IdFor(firstType)}</id>
            <name>{FullyQualifiedName(firstType, false, false)}</name>
            <type>{GetKindString(firstType)}</type>
            <namespace>{GetNamespace(firstType)}</namespace>
            <relativeFilePaths>
                {GetPaths(group)}
            </relativeFilePaths>
        </type>");
            }

            output.Append("</types></doc>");

            return output.ToString();
        }

        private string IdFor(INamedTypeSymbol symbol)
        {
            return FullyQualifiedName(symbol, true, true);
        }
        
        private static string FullyQualifiedName(ISymbol t, bool includeNamespace, bool useMetadataName)
        {
            return t.QualifiedName(includeNamespace, useMetadataName);
        }

        private static string GetKindString(INamedTypeSymbol typeSymbol)
        {
            return typeSymbol.TypeKind.ToString();
        }

        private string GetPaths(IGrouping<string, INamedTypeSymbol> group)
        {
            var paths = group.SelectMany(t => t.Locations).Select(l => l.SourceTree.FilePath).Distinct();
            return paths.Aggregate("", (acc, curr) => acc + $"<path>{curr}</path>\r\n");
        }

        private string GetNamespace(INamespaceOrTypeSymbol t)
        {
            return FullyQualifiedName(t.ContainingNamespace, true, false);
        }

        internal void Visit(SyntaxNode syntaxNode, SemanticModel semanticModel)
        {
            this.semanticModel = semanticModel;
            this.Visit(syntaxNode);
        }
    }
}