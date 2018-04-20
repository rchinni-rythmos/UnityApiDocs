using Unity.DocTool.XMLDocHandler.Tests.TestTypes.Attributes;

namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes
{
    class ClassWithProperty
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
