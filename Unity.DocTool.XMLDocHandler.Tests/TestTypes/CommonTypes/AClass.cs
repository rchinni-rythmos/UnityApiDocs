﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes
{
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
            throw new NotImplementedException();
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Private method
        /// </summary>
        private void DoNotReport()
        {}
    }

    public partial class AClass
    {

    }
}
