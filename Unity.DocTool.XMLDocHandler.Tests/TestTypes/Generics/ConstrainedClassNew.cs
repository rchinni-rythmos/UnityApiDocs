using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes.Generics
{
    class ConstrainedClassNew<T> where T : class, new()
    {
        void Foo()
        {

        }
    }

    class ExtendsInterface : IEnumerable<int>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator<int> IEnumerable<int>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
