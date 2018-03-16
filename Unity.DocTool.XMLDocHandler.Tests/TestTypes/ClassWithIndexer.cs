using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes
{
    class ClassWithIndexer
    {
        /// <summary>
        /// Indexer property
        /// </summary>
        public int this[int a]
        {
            get
            {
                return 1;
            }
            protected set { }
        }
    }
}
