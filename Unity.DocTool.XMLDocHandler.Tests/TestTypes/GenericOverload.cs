namespace DocWorks.Integration.XmlDoc.Tests.TestTypes
{
    public struct GenericOverload
    {
        /// <summary>
        /// Existing GenericMethod-T
        /// </summary>
        public void GenericMethod<T>()
        { }
        /// <summary>
        /// Existing GenericMethod
        /// </summary>
        public void GenericMethod()
        { }
    }
}