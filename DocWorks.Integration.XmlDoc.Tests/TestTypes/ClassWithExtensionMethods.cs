using DocWorks.Integration.XmlDoc.Tests.TestTypes.Attributes;

namespace DocWorks.Integration.XmlDoc.Tests.TestTypes
{
    public static class ClassWithExtensionMethods
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
