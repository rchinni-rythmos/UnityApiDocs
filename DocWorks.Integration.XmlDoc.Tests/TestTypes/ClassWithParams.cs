using DocWorks.Integration.XmlDoc.Tests.TestTypes.Attributes;

namespace DocWorks.Integration.XmlDoc.Tests.TestTypes
{
    public class ClassWithParams
    {
        /// <summary>
        /// Params field
        /// </summary>
        public void Method(params int[] ints) { }
    }
}
