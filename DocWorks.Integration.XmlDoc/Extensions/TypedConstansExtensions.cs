using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace DocWorks.Integration.XmlDoc.Extensions
{
    public static class TypedConstantExtensions
    {
        public static string ToCSharpStringNoStringQuotes(this TypedConstant constant)
        {
            if (constant.Type.QualifiedName(true, NameFormat.MetadataName) == "System.String")
                return XmlUtility.EscapeString(SymbolDisplay.FormatPrimitive(constant.Value, false, false));

            return constant.ToCSharpString();
        }
    }
}
