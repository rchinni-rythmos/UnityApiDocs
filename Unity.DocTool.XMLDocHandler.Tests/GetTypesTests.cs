using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace Unity.DocTool.XMLDocHandler.Tests
{
    [TestFixture]
    public class XmlDocHandlerTest : XmlDocHandlerTestBase
    {
        [Test]
        public void GetTypes_Returns_Relative_Path_When_Given_Path_Without_Trailing_Slash()
        {
            var testFileDirectory = TestPathFor("TestTypes/CommonTypes");
            var handler = new XMLDocHandler(MakeCompilationParameters(testFileDirectory));
            string xmlActual = handler.GetTypesXml();

            Assert.That(xmlActual, Contains.Substring("<path>AClass.cs</path>"));
            Assert.That(xmlActual, Contains.Substring(@"<path>AFolder\AClass.part2.cs</path>"));
        }

        [Test]
        public void GetTypes_Full_ReturnsCorrectXml()
        {
            var testFileDirectory = TestPathFor("TestTypes/CommonTypes/");
            var handler = new XMLDocHandler(MakeCompilationParameters(testFileDirectory));
            string xmlActual = handler.GetTypesXml();

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
        public void GetTypes_Generics_ReturnsCorrectXml()
        {
            var testFileDirectory = TestPathFor("TestTypes/Generics/");
            var handler = new XMLDocHandler(MakeCompilationParameters(testFileDirectory));
            string xmlActual = handler.GetTypesXml();

            var expected = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<doc version=""1"">
    <types>
        <type>
            <id>Unity.DocTool.XMLDocHandler.Tests.TestTypes.Generics.GenericClass</id>
            <parentId></parentId>
            <name>GenericClass</name>
            <kind>Class</kind>
            <namespace>Unity.DocTool.XMLDocHandler.Tests.TestTypes.Generics</namespace>
            <relativeFilePaths>
                <path>GenericClass.cs</path>

            </relativeFilePaths>
        </type>
        <type>
            <id>Unity.DocTool.XMLDocHandler.Tests.TestTypes.Generics.GenericClass`1</id>
            <parentId></parentId>
            <name>GenericClass</name>
            <kind>Class</kind>
            <namespace>Unity.DocTool.XMLDocHandler.Tests.TestTypes.Generics</namespace>
            <relativeFilePaths>
                <path>GenericClass.cs</path>

            </relativeFilePaths>
        </type>
        <type>
            <id>Unity.DocTool.XMLDocHandler.Tests.TestTypes.Generics.GenericClassWithConstraints`1</id>
            <parentId></parentId>
            <name>GenericClassWithConstraints</name>
            <kind>Class</kind>
            <namespace>Unity.DocTool.XMLDocHandler.Tests.TestTypes.Generics</namespace>
            <relativeFilePaths>
                <path>GenericClass.cs</path>

            </relativeFilePaths>
        </type>
        <type>
            <id>Unity.DocTool.XMLDocHandler.Tests.TestTypes.Generics.ExtendsInterface</id>
            <parentId></parentId>
            <name>ExtendsInterface</name>
            <kind>Class</kind>
            <namespace>Unity.DocTool.XMLDocHandler.Tests.TestTypes.Generics</namespace>
            <relativeFilePaths>
                <path>GenericClass.cs</path>

            </relativeFilePaths>
        </type></types></doc>";

            AssertXml(expected, xmlActual);
        }

        [Test]
        public void GetType_Documentation_ReturnsCorrectXml()
        {
            var handler = new XMLDocHandler(MakeCompilationParameters("TestTypes/CommonTypes/"));
            string actualXml = handler.GetTypeDocumentation("Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes.AClass",  "AClass.cs", "AFolder/AClass.part2.cs");
            Console.WriteLine(actualXml);

            var expectedXml = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<doc version=""3"">
    <member name=""AClass"" type = ""Class"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes"" inherits=""Object"">
        <interfaces>
            <interface typeId=""System.Collections.IEnumerable"" typeName=""IEnumerable"" />
            <interface typeId=""System.ICloneable"" typeName=""ICloneable"" />
        </interfaces>
        <xmldoc><![CDATA[
            <summary>I have a summary</summary>
            <example>In a partial type...</example>
            Here is some more docs
        ]]></xmldoc>

        <member name = ""Foo"" type=""Method"">
            <signature>
                <accessibility>Public</accessibility>
                <return typeId=""System.Int32"" typeName=""int"" />
                <parameters>
                    <parameter name=""i"" typeId=""System.Int32"" typeName=""int"" />
                </parameters>
            </signature>
            <xmldoc><![CDATA[
                <summary>So do I</summary>
                <returns>whatever you want.</returns>
            ]]></xmldoc>                
        </member>

        <member name = ""VoidProtectedMethod"" type=""Method"">
            <signature>
                <accessibility>Protected</accessibility>
                <return typeId=""System.Void"" typeName=""void"" />
                <parameters>
                </parameters>
            </signature>
            <xmldoc><![CDATA[
                some docs
            ]]></xmldoc>                
        </member>
        <member name = ""System.Collections.IEnumerable.GetEnumerator"" type=""Method"">
            <signature>
                <accessibility>Private</accessibility>
                <return typeId=""System.Collections.IEnumerator"" typeName=""System.Collections.IEnumerator"" />
                <parameters></parameters>
            </signature>
            <xmldoc><![CDATA[
                <summary>
                Explicit Implementation
                </summary>
                <returns></returns>
            ]]></xmldoc>
        </member>
        <member name = ""Clone"" type=""Method"">
            <signature>
                <accessibility>Public</accessibility>
                <return typeId=""System.Object"" typeName=""object"" />
                <parameters></parameters>
            </signature>
            <xmldoc><![CDATA[    
            ]]></xmldoc>
        </member>
    </member>
</doc>";
            AssertXml(expectedXml, actualXml);
        }

        [Test]
        public void Test_Documentation_Under_Conditional_Compilation_Symbols_Works()
        {
            Assert.Inconclusive("Not implementated yet");
        }

        [Test]
        public void Test_Inner_Types()
        {
            var handler = new XMLDocHandler(MakeCompilationParameters("TestTypes/CommonTypes/"));
            string actualXml = handler.GetTypeDocumentation("Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes.AClass.INestedInterface", "AClass.cs");
            Console.WriteLine(actualXml);

            var expectedXml = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
<doc version=""3"">
    <member name=""INestedInterface"" type = ""Interface"" containingType=""AClass"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes"" inherits="""">
        <xmldoc><![CDATA[
            <summary>
            I am a nested interface
            </summary>
        ]]></xmldoc>
    </member>
</doc>";
            AssertXml(expectedXml, actualXml);
        }

        [Test]
        public void Test_Property_Is_Reported()
        {
            var handler = new XMLDocHandler(MakeCompilationParameters("TestTypes/"));
            string actualXml = handler.GetTypeDocumentation("Unity.DocTool.XMLDocHandler.Tests.TestTypes.ClassWithProperty", "ClassWithProperty.cs");
            Console.WriteLine(actualXml);

            string expectedXml = @"<member name = ""Value"" type=""Property"">
<signature>
    <accessibility>Public</accessibility>
    <type typeId=""System.Int32"" typeName=""int"" />
    <get><accessibility>Public</accessibility></get>
    <parameters></parameters>
</signature>
<xmldoc><![CDATA[
                
    <summary>
    Value property
    </summary>


]]></xmldoc>
</member>";

            Assert.That(Normalize(actualXml), Contains.Substring(Normalize(expectedXml)), actualXml);
        }

        [Test]
        public void Test_PropertyWithIndexer_IsReported()
        {
            var handler = new XMLDocHandler(MakeCompilationParameters("TestTypes/"));
            string actualXml = handler.GetTypeDocumentation("Unity.DocTool.XMLDocHandler.Tests.TestTypes.ClassWithIndexer", "ClassWithIndexer.cs");
            Console.WriteLine(actualXml);

            string expectedXml = @"  <?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
    <doc version=""3"">
        <member name=""ClassWithIndexer"" type = ""Class"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes"" inherits=""Object"">
        
        <xmldoc><![CDATA[
        
        ]]></xmldoc><member name = ""this[]"" type=""Property"">
            <signature>
<accessibility>Public</accessibility>
<type typeId=""System.Int32"" typeName=""int"" />

<get><accessibility>Public</accessibility></get>
<set><accessibility>Protected</accessibility></set>
<parameters><parameter name=""a"" typeId=""System.Int32"" typeName=""int"" />
</parameters></signature>
            <xmldoc><![CDATA[
                
    <summary>
    Indexer property
    </summary>


            ]]></xmldoc>
        </member>
</member></doc>";
            
            AssertXml(expectedXml, actualXml);
        }

        [Test]
        public void Test_Attributes_Are_Reported()
        {
            Assert.Inconclusive("Not implementated yet");
        }

        [Test]
        public void Test_Generic_Types()
        {
            var handler = new XMLDocHandler(MakeCompilationParameters("TestTypes/Generics"));
            string actualXml = handler.GetTypeDocumentation("Unity.DocTool.XMLDocHandler.Tests.TestTypes.Generics.GenericClass`1", "GenericClass.cs");
            Console.WriteLine(actualXml);

            string expectedXml = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
    <doc version=""3"">
        <member name=""GenericClass`1"" type = ""Class"" namespace=""Unity.DocTool.XMLDocHandler.Tests.TestTypes.Generics"" inherits=""Object"">
        <xmldoc>
            <![CDATA[
    <summary>
    Existing Docs for GenericClass-T
    </summary>

]]>
        </xmldoc><member name = ""Foo"" type=""Method"">
            <signature>
<accessibility>Public</accessibility>
<return typeId=""System.Void"" typeName=""void"" />
<parameters></parameters></signature>
            <xmldoc>
                <![CDATA[
    <summary>
    Existing GenericClass-T.Foo
    </summary>

]]>
            </xmldoc>
        </member>
</member></doc>";
            AssertXml(expectedXml, actualXml);
        }

        [Test]
        public void Test_Generic_Types_With_Constraints()
        {
            Assert.Inconclusive("Not implementated yet. Should expose enough information to produce consumer docs.");
        }

        [Test]
        public void Test_Generic_Members()
        {
            Assert.Inconclusive("Not implementated yet");
        }

        private void AssertXml(string expectedXml, string actualXml)
        {
            var normalizedExpectedXml = Normalize(expectedXml);
            var normalizedActualXml = Normalize(actualXml);

            Assert.AreEqual(normalizedExpectedXml, normalizedActualXml, actualXml);
        }

        private static string TestPathFor(string path)
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), path);
        }
    }
}
