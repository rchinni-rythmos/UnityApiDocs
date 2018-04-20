using System;
using System.Collections;
using System.Collections.Generic;

namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes.Generics
{
    /// <summary>
    /// Existing Docs for ExtendsGenericInterface
    /// </summary>
    public class ExtendsGenericInterface : IEnumerable<int>
    {
        /// <summary>
        /// Existing Docs for IEnumerable-int.GetEnumerator()
        /// </summary>
        IEnumerator<int> IEnumerable<int>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Existing Docs for IEnumerable.GetEnumerator()
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}