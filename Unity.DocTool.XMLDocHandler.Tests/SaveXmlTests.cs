using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Unity.DocTool.XMLDocHandler.Tests
{
    [TestFixture]
    class SaveXmlTests : XmlDocHandlerTestBase
    {
        [Test]
        public void SavesBackToSource()
        {
            var handler = new XMLDocHandler(MakeCompilationParameters("TestTypes/CommonTypes/"));
            string newDocXml = "";
            handler.SetType(newDocXml, "AnEnum.cs");
            var actualSource = File.ReadAllText("TestTypes/CommonTypes/AnEnum.cs");
            var expectedSource = @"
    /// <summary>
    /// Some Docs
    /// </summary>
    public enum AnEnum
    {
    }";
            AssertSourceContains(expectedSource, actualSource);
        }

        private void AssertSourceContains(string expectedSource, string actualSource)
        {
            Assert.IsTrue(Normalize(actualSource).Contains(Normalize(expectedSource)),
                $"Expected\n {expectedSource}\nbut got\n {actualSource}");
        }
    }
}
