using System;
using System.Collections.Generic;
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

        private static HashSet<MethodKind> implicitMethodKinds = new HashSet<MethodKind>
            {
                MethodKind.AnonymousFunction,
                MethodKind.EventAdd,
                MethodKind.EventRemove,
                MethodKind.EventRaise,
                MethodKind.PropertyGet,
                MethodKind.PropertySet,
                MethodKind.LocalFunction,
                MethodKind.DelegateInvoke
            };

        internal static bool MayHaveXmlDoc(this ISymbol symbol)
        {
            if (symbol is IMethodSymbol methodSymbol)
            {
                switch (methodSymbol.MethodKind)
                {
                    case MethodKind.Ordinary:
                    case MethodKind.UserDefinedOperator:
                    case MethodKind.Destructor:
                    case MethodKind.Constructor:
                    case MethodKind.Conversion:
                        break;
                    case MethodKind.StaticConstructor:
                    case MethodKind.EventAdd:
                    case MethodKind.EventRemove:
                    case MethodKind.EventRaise:
                    case MethodKind.PropertyGet:
                    case MethodKind.PropertySet:
                        return false;
                    case MethodKind.ExplicitInterfaceImplementation:
                        return true;
                    case MethodKind.AnonymousFunction:
                    case MethodKind.LocalFunction:
                    case MethodKind.DelegateInvoke:
                    case MethodKind.BuiltinOperator:
                    case MethodKind.ReducedExtension:
                    case MethodKind.DeclareMethod:
                        throw new NotSupportedException($"Unsupported MethodKind: {methodSymbol.MethodKind}");
                }
            }

            return IsPublicApi(symbol);
        }

        public static bool IsPublicApi(this ISymbol symbol)
        {
            //TODO: Take nested types into account.           
            var accessibility = symbol.DeclaredAccessibility;
            return accessibility == Accessibility.Public ||
                   accessibility == Accessibility.Protected ||
                   accessibility == Accessibility.ProtectedAndInternal;
        }
    }
}
