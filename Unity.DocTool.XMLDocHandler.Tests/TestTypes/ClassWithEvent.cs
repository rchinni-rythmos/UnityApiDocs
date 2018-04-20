using Unity.DocTool.XMLDocHandler.Tests.TestTypes.Attributes;

namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes
{
    class ClassWithEvent
    {
        /// <summary>
        /// anEvent
        /// </summary>
        [TestInternal] [TestPublic]
        public event System.Action<bool> anEvent;
    }
}
