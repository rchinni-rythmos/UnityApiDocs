using DocWorks.Integration.XmlDoc.Tests.TestTypes.Attributes;

namespace DocWorks.Integration.XmlDoc.Tests.TestTypes
{
    public class ClassWithProperty
    {
        /// <summary>
        /// Value property
        /// </summary>
        [TestInternal]
        [TestPublic]
        public int Value
        {
            get;
            private set;
        }
    }
}
