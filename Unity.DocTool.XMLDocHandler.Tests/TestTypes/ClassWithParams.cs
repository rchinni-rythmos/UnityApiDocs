using Unity.DocTool.XMLDocHandler.Tests.TestTypes.Attributes;

namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes
{
    public class ClassWithParams
    {
        /// <summary>
        /// Params field
        /// </summary>
        public void Method(params int[] ints) { }
    }
}
