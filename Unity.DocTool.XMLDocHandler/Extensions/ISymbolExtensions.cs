using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Unity.DocTool.XMLDocHandler.Extensions
{
    public static class ISymbolExtensions
    {
        public static string QualifiedName(this ISymbol symbol, bool includeNamespace, bool useMetadataName)
        {
            if (symbol == null)
                return null;

            if (symbol is IArrayTypeSymbol)
            {
                var arraySymbol = (IArrayTypeSymbol) symbol;
                var elementName = QualifiedName(arraySymbol.ElementType, includeNamespace, useMetadataName);
                return $"{elementName}[{string.Join(" ", Enumerable.Repeat(',', arraySymbol.Rank - 1))}]";
            }

            string name;
            if (useMetadataName)
            {
                var methodSymbol = symbol as IMethodSymbol;
                if (methodSymbol != null)
                    name = methodSymbol.MemberName();
                else
                    name = symbol.MetadataName;
            }
            else
                name = symbol.Name;

            string prefix = null;
            if (symbol.ContainingType != null)
                prefix = QualifiedName(symbol.ContainingType, includeNamespace, useMetadataName);
            else if (includeNamespace && !symbol.ContainingNamespace.IsGlobalNamespace)
                prefix = QualifiedName(symbol.ContainingNamespace, includeNamespace, useMetadataName);

            string separator = symbol is IMethodSymbol ? "::" : ".";

            return prefix != null ? prefix + separator + name : name;
        }


        internal static string Id(this ISymbol symbol)
        {
            if (symbol is IMethodSymbol)
                return Id((IMethodSymbol)symbol);

            return FullyQualifiedName(symbol, true, true);
        }

        internal static string Id(this IMethodSymbol symbol)
        {
            string id = $@"{symbol.ReturnType.Id()} {FullyQualifiedName(symbol, true, true)}({string.Join(", ", symbol.Parameters.Select(p => p.Type.Id()))})";
            if (symbol.IsStatic)
                id = "static " + id;

            return id;
        }

        internal static string FullyQualifiedName(this ISymbol t, bool includeNamespace, bool useMetadataName)
        {
            return t.QualifiedName(includeNamespace, useMetadataName);
        }

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
            if (symbol.ContainingType != null && !IsPublicApi(symbol.ContainingType))
                return false;

            var accessibility = symbol.DeclaredAccessibility;
            return accessibility == Accessibility.Public ||
                   accessibility == Accessibility.Protected ||
                   accessibility == Accessibility.ProtectedAndInternal;
        }

        public static string MemberName(this ISymbol member)
        {
            return XmlUtility.EscapeString(member.MemberNameUnescaped());
        }

        public static string MemberNameUnescaped(this ISymbol member)
        {
            var memberName = member.Name;
            if (member.Kind == SymbolKind.Method)
            {
                var methodSymbol = (IMethodSymbol)member;
                if (methodSymbol.TypeParameters.Length > 0)
                    memberName += "`" + methodSymbol.TypeParameters.Length;
            }

            return memberName;
        }
    }
}
