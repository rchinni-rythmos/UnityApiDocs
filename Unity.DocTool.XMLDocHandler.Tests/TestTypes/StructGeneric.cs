namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes
{
    struct StructWithGeneric<T>
    {
        /// <summary>
        /// Value property
        /// </summary>
        public int Value
        {
            get;
            private set;
        }
    }
}
