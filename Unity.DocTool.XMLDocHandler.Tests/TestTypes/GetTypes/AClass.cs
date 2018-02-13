using System;
using System.Collections.Generic;
using System.Text;

namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes
{
    /// <summary>
    /// I have a summary
    /// </summary>
    public partial class AClass
    {
        public interface INestedInterface
        {
            
        }

        private class APrivateClass
        {
        }

        /// <summary>
        /// So do I
        /// </summary>
        /// <returns>whatever you want.</returns>
        public int Foo(int i)
        {
            return i;
        }
    }

    public partial class AClass
    {

    }
}
