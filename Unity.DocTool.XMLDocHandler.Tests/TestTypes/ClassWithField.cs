using DocWorks.Integration.XmlDoc.Tests.TestTypes.Attributes;

namespace DocWorks.Integration.XmlDoc.Tests.TestTypes
{
    public class ClassWithField
    {
        /// <summary>
        /// Value field
        /// </summary>
        [TestInternal]
        [TestPublic]
        public int value;
    }
}
