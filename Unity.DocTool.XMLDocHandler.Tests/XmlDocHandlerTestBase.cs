using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Unity.DocTool.XMLDocHandler.Tests
{
    internal class XmlDocHandlerTestBase
    {
        private string originalCurrentDirectory;

        [SetUp]
        public void Init()
        {
            originalCurrentDirectory = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(TestContext.CurrentContext.TestDirectory);
        }

        [TearDown]
        public void Cleanup()
        {
            Directory.SetCurrentDirectory(originalCurrentDirectory);
        }

        protected static CompilationParameters MakeCompilationParameters(string testFileDirectory)
        {
            return new CompilationParameters(testFileDirectory, new string[0], new []
            {
                typeof(object).Assembly.Location,
            });
        }

        protected static string Normalize(string source)
        {
            return Regex.Replace(source, @"\r|\n|\s{2,}", "");
        }

        protected void AssertXmlContains(string expectedXml, string actualXml)
        {
            var normalizedExpectedXml = Normalize(expectedXml);
            var firstExpectedEnd = normalizedExpectedXml.IndexOf(">");
            var firstTag = normalizedExpectedXml.Substring(0, firstExpectedEnd);

            var normalizedActualXml = Normalize(actualXml);
            var indexOfStart = normalizedActualXml.IndexOf(firstTag);
            Assert.AreNotEqual(-1, indexOfStart, "Could not find \"" + firstTag + "\" in \"" + actualXml + "\"");
            var actualXmlExpectedToMatch = normalizedActualXml.Substring(indexOfStart,
                Math.Min(normalizedActualXml.Length - indexOfStart, normalizedExpectedXml.Length));
            AssertXml(normalizedExpectedXml, actualXmlExpectedToMatch);
        }

        protected void AssertXml(string expectedXml, string actualXml)
        {
            var normalizedExpectedXml = Normalize(expectedXml);
            var normalizedActualXml = Normalize(actualXml);

            Assert.AreEqual(normalizedExpectedXml, normalizedActualXml, actualXml);
        }
    }
}