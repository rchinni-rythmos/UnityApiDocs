namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes
{
    class ClassWithEventAddRemove
    {
        /// <summary>
        /// anEvent
        /// </summary>
        public event System.Action<System.Func<bool>> anEvent
        {
            add { }
            remove { }
        }
    }
}
