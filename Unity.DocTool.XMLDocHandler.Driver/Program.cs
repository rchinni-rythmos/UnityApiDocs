namespace Unity.DocTool.XMLDocHandler.Driver
{
    /// <summary>
    /// My summary
    /// </summary>
    /// <remarks>
    /// My  Remarks
    /// </remarks>
    /// <code>
    /// fgfgfg
    /// </code>
    class Program
    {
        /// <summary>
        /// The main thing
        /// </summary>
        /// <param name="args">the args</param>
        static void Main(string[] args)
        {
            var handler = new XMLDocHandler();
            var path = @"G:\Work\repo\xmldochandler\Unity.DocTool.XMLDocHandler.Driver\Program.cs";
            handler.GetComments(path);
            //handler.UpdateComments(path);
        }

        /// <summary>
        /// Name
        /// </summary>
        /// <value>
        /// Value
        /// </value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// M Generic
        /// </summary>
        /// <typeparam name="T">TypeParam T</typeparam>
        /// <returns>M&lt;T&gt;Return</returns>
        public T M<T>()
        {
            return default(T);
        }

        public class MyInnerType
        {
            /// <summary>
            /// Of course we do
            /// </summary>
            public void Do() { }
        }
    }
}
