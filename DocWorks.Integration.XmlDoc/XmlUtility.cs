using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DocWorks.Integration.XmlDoc
{
    static class XmlUtility
    {
        public static string EscapeString(string xmlString)
        {
            Regex rgx = new Regex(@"&(?!amp;)(?!lt;)(?!gt;)(?!quot;)(?!apos;)");
            xmlString = rgx.Replace(xmlString, "&amp;");
            return xmlString
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&apos;");
        }

        public static string LegalString(string xmlString)
        {
            return xmlString
                .Replace("&amp;", "&")
                .Replace("&lt;", "<")
                .Replace("&gt;", ">")
                .Replace("&quot;", "\"")
                .Replace("&apos;", "'");
        }
    }
}
