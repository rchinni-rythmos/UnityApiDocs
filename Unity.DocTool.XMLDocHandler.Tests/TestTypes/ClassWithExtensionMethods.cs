using Unity.DocTool.XMLDocHandler.Tests.TestTypes.Attributes;

namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes
{
    static class ClassWithExtensionMethods
    {
        /// <summary>
        /// Extension method
        /// </summary>
        [TestInternal]
        [TestPublic]
        public static int ExtensionMethod(this string s)
        {
            return 0;
        }
    }
}
