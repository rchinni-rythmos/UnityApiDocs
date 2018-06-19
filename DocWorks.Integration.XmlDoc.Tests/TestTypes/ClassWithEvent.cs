using DocWorks.Integration.XmlDoc.Tests.TestTypes.Attributes;

namespace DocWorks.Integration.XmlDoc.Tests.TestTypes
{
    public class ClassWithEvent
    {
        /// <summary>
        /// anEvent
        /// </summary>
        [TestInternal] [TestPublic]
        public event System.Action<bool> anEvent;
    }
}
