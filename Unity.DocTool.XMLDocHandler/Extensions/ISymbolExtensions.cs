using Microsoft.CodeAnalysis;

namespace Unity.DocTool.XMLDocHandler.Extensions
{
    public static class ISymbolExtensions
    {
        public static string QualifiedName(this ISymbol symbol, bool includeNamespace, bool useMetadataName)
        {
            if (symbol == null)
                return null;

            var name = useMetadataName ? symbol.MetadataName : symbol.Name;

            string prefix = null;
            if (symbol.ContainingType != null)
                prefix = QualifiedName(symbol.ContainingType, includeNamespace, useMetadataName);
            else if (includeNamespace && !symbol.ContainingNamespace.IsGlobalNamespace)
                prefix = QualifiedName(symbol.ContainingNamespace, includeNamespace, useMetadataName);

            return prefix != null ? prefix + "." + name : name;
        }

        internal static string Id(this ISymbol symbol)
        {
            return FullyQualifiedName(symbol, true, true);
        }

        internal static string FullyQualifiedName(this ISymbol t, bool includeNamespace, bool useMetadataName)
        {
            return t.QualifiedName(includeNamespace, useMetadataName);
        }

        internal static bool IsPublicApi(this ISymbol symbol)
        {
            var accessibility = symbol.DeclaredAccessibility;
            var isExplicitImplementation = symbol is IMethodSymbol methodSymbol && methodSymbol.MethodKind == MethodKind.ExplicitInterfaceImplementation;
            return isExplicitImplementation ||
                   accessibility == Accessibility.Public ||
                   accessibility == Accessibility.Protected ||
                   accessibility == Accessibility.ProtectedAndInternal;
        }
    }
}
