using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Unity.DocTool.XMLDocHandler.Tests
{
    [TestFixture]
    public class GetTypesTests
    {
        [Test]
        public void GetTypes_Full_ReturnsCorrectXml()
        {
            var handler = new XMLDocHandler();
            var testFileDirectory = TestPathFor("TestTypes/GetTypes/");
            string xmlActual = handler.GetTypesXml(testFileDirectory);

            var expected = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<doc version=""1"">
    <types>
        <type>
            <name>AClass</name>
            <type>Class</type>
            <namespace>Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes</namespace>
            <relativeFilePaths>
                <path>AClass.cs</path>
<path>AFolder\AClass.part2.cs</path>

            </relativeFilePaths>
        </type>
        <type>
            <name>AClass.INestedInterface</name>
            <type>Interface</type>
            <namespace>Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes</namespace>
            <relativeFilePaths>
                <path>AClass.cs</path>

            </relativeFilePaths>
        </type>
        <type>
            <name>AnEnum</name>
            <type>Enum</type>
            <namespace>Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes</namespace>
            <relativeFilePaths>
                <path>AnEnum.cs</path>

            </relativeFilePaths>
        </type>
        <type>
            <name>AClass.AnEnum</name>
            <type>Enum</type>
            <namespace>Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes</namespace>
            <relativeFilePaths>
                <path>AFolder\AClass.part2.cs</path>

            </relativeFilePaths>
        </type></types></doc>";

            AssertXml(expected, xmlActual);

        }

        [Test]
        public void GetType_Documentation_ReturnsCorrectXml()
        {
            var currrentDirectory = Directory.GetCurrentDirectory();
            try
            {
                var testRootFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                Directory.SetCurrentDirectory(testRootFolder);

                var handler = new XMLDocHandler();
                string actualXml = handler.GetTypeDocumentation("Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes.AClass", "TestTypes/GetTypes/",  "AClass.cs", "AFolder/AClass.part2.cs");
                Console.WriteLine(actualXml);

                var expectedXml = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<doc version=""3"">
    <member name=""AClass"" type = ""Class"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes"" inherits=""object"">
        <xmldoc>
            <summary>I have a summary</summary>
            <example>In a partial type...</example>
            Here is some more docs
        </xmldoc>

        <member name = ""Foo"" type=""Method"">
            <signature>
                <return typeId=""System.Int32"" typeName=""int"" />
                <parameters>
                    <parameter name=""i"" typeId=""System.Int32"" typeName=""int"" />
                </parameters>
            </signature>
            <xmldoc>
                <summary>So do I</summary>
                <returns>whatever you want.</returns>
            </xmldoc>                
        </member>

        <member name = "".ctor"" type=""Method"">
            <signature>
                <parameters></parameters>
            </signature>
            <xmldoc></xmldoc>
        </member>
    </member>
</doc>";
                AssertXml(expectedXml, actualXml);

            }
            finally
            {
                Directory.SetCurrentDirectory(currrentDirectory);
            }

        }

        private void AssertXml(string expectedXml, string actualXml)
        {
            var normalizedExpectedXml = NormalizeXml(expectedXml);
            var normalizedActualXml = NormalizeXml(actualXml);

            Assert.AreEqual(normalizedExpectedXml, normalizedActualXml, actualXml);
        }

        private static string NormalizeXml(string xml)
        {
            return Regex.Replace(xml, @"\r|\n|\s{2,}", "");
        }

        private static string TestPathFor(string path)
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), path);
        }
    }
}
