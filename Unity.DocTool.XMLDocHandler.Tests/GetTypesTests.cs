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
        private string originalCurrentDirectory;

        [SetUp]
        public void Init()
        {
            originalCurrentDirectory = Directory.GetCurrentDirectory();
            var testRootFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Directory.SetCurrentDirectory(testRootFolder);
        }

        [TearDown]
        public void Cleanup()
        {
            Directory.SetCurrentDirectory(originalCurrentDirectory);
        }

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
            <id>Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes.AClass</id>
            <parentId></parentId>
            <name>AClass</name>
            <kind>Class</kind>
            <namespace>Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes</namespace>
            <relativeFilePaths>
                <path>AClass.cs</path>
<path>AFolder\AClass.part2.cs</path>

            </relativeFilePaths>
        </type>
        <type>
            <id>Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes.AClass.INestedInterface</id>
            <parentId>Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes.AClass</parentId>
            <name>INestedInterface</name>
            <kind>Interface</kind>
            <namespace>Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes</namespace>
            <relativeFilePaths>
                <path>AClass.cs</path>

            </relativeFilePaths>
        </type>
        <type>
            <id>Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes.AnEnum</id>
            <parentId></parentId>
            <name>AnEnum</name>
            <kind>Enum</kind>
            <namespace>Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes</namespace>
            <relativeFilePaths>
                <path>AnEnum.cs</path>

            </relativeFilePaths>
        </type>
        <type>
            <id>Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes.AClass.AnEnum</id>
            <parentId>Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes.AClass</parentId>
            <name>AnEnum</name>
            <kind>Enum</kind>
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
            var handler = new XMLDocHandler();
            string actualXml = handler.GetTypeDocumentation("Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes.AClass", new string[0], "TestTypes/GetTypes/",  "AClass.cs", "AFolder/AClass.part2.cs");
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

        [Test]
        public void Test_Documentation_Under_Conditional_Compilation_Symbols_Works()
        {
            Assert.Fail("Not implementated yet");
        }

        [Test]
        public void Test_Inner_Types()
        {
            var handler = new XMLDocHandler();
            string actualXml = handler.GetTypeDocumentation("Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes.AClass.INestedInterface", new string[0], "TestTypes/GetTypes/", "AClass.cs");
            Console.WriteLine(actualXml);

            var expectedXml = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<doc version=""3"">
    <member name=""INestedInterface"" type = ""Interface"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes"" inherits="""">
        <xmldoc>
            <summary>
            I am a nested interface
            </summary>
        </xmldoc>
    </member>
</doc>";
            AssertXml(expectedXml, actualXml);
        }

        [Test]
        public void Test_Attributes_Are_Reported()
        {
            Assert.Fail("Not implementated yet");
        }

        [Test]
        public void Test_Generic_Types()
        {
            Assert.Fail("Not implementated yet");
        }

        [Test]
        public void Test_Generic_Members()
        {
            Assert.Fail("Not implementated yet");
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
