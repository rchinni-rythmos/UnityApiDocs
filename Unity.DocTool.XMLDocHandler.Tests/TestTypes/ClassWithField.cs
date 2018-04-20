using Unity.DocTool.XMLDocHandler.Tests.TestTypes.Attributes;

namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes
{
    class ClassWithField
    {
        /// <summary>
        /// Value field
        /// </summary>
        [TestInternal]
        [TestPublic]
        public int value;
    }
}
