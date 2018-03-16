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
            if (AddTypeIfPublicApi(node))
                base.VisitClassDeclaration(node);
        }

        public override void VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            if (AddTypeIfPublicApi(node))
                base.VisitEnumDeclaration(node);
        }

        public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            if (AddTypeIfPublicApi(node))
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
            if (AddTypeIfPublicApi(node))
                base.VisitStructDeclaration(node);
        }

        private bool AddTypeIfPublicApi(BaseTypeDeclarationSyntax node)
        {
            var symbol = semanticModel.GetDeclaredSymbol(node);
            if (symbol.IsPublicApi())
            {
                types.Add(symbol);
                return true;
            }

            return false;
        }

        public string GetXml()
        {
            var groups = types.GroupBy(t => t.FullyQualifiedName(true, false));
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
            <id>{firstType.Id()}</id>
            <parentId>{firstType.ContainingType?.Id()}</parentId>
            <name>{firstType.Name}</name>
            <kind>{GetKindString(firstType)}</kind>
            <namespace>{GetNamespace(firstType)}</namespace>
            <relativeFilePaths>
                {GetPaths(group)}
            </relativeFilePaths>
        </type>");
            }

            output.Append("</types></doc>");

            return output.ToString();
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
            return t.ContainingNamespace.FullyQualifiedName(true, false);
        }

        internal void Visit(SyntaxNode syntaxNode, SemanticModel semanticModel)
        {
            this.semanticModel = semanticModel;
            this.Visit(syntaxNode);
        }
    }
}