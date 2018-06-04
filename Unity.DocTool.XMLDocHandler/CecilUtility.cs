using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Unity.DocTool.XMLDocHandler
{
    class CecilUtility
    {
        public static bool HasXmlDocs(SyntaxNode node)
        {
            if (!node.HasLeadingTrivia)
                return false;

            foreach (var trivia in node.GetLeadingTrivia())
            {
                if (trivia.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia))
                    return true;
            }

            return false;
        }
    }
}
