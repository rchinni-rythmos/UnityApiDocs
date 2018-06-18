using System;
using System.Collections;

namespace DocWorks.Integration.XmlDoc.Tests.TestTypes.GetTypes
{
    public partial class AClass { }

    /// <summary>
    /// I have a summary
    /// </summary>
    public partial class AClass : IEnumerable, ICloneable
    {
        /// <summary>
        /// I am a nested interface
        /// </summary>
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

        /// some docs
        protected void VoidProtectedMethod() { }

        /// <summary>
        /// Explicit Implementation
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return null;
        }

        public object Clone()
        {
            return null;
        }

        /// <summary>
        /// Private method
        /// </summary>
        private void DoNotReport()
        {}

        /// <summary>
        /// Delegate
        /// </summary>
        public delegate void Delegate();
    }
}
