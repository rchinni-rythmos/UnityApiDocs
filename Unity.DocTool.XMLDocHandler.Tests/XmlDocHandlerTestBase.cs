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
    }
}