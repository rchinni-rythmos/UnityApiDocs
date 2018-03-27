using System;
using System.Collections;
using System.Collections.Generic;
using Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes;

namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes.Generics
{
    /// <summary>
    /// Existing Docs for GenericClass
    /// </summary>
    public class GenericClass
    {
        /// <summary>
        /// Existing Docs for GenericClass.Foo
        /// </summary>
        public void Foo()
        {}
    }
    /// <summary>
    /// Existing Docs for GenericClass-T
    /// </summary>
    public class GenericClass<T>
    {
        /// <summary>
        /// Existing GenericClass-T.Foo
        /// </summary>
        public void Foo()
        {}
    }

    /// <summary>
    /// Existing Docs for GenericClassWithConstraints-T
    /// </summary>
    public class GenericClassWithConstraints<T> where T : class, IList<AClass>, new()
    { }

    /// <summary>
    /// Existing Docs for ExtendsInterface
    /// </summary>
    public class ExtendsInterface : IEnumerable<int>
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
