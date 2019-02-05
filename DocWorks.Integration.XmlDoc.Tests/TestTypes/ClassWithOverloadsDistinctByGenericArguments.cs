using System.Collections.Generic;

namespace DocWorks.Integration.XmlDoc.Tests.TestTypes
{
    public class ClassWithOverloadsDistinctByGenericArguments
    {
        /// <summary>
        /// Method(IEnumerable<int>)
        /// </summary>
        public void Method(IEnumerable<int> enumerable) { }
        /// <summary>
        /// Method(IEnumerable<byte>)
        /// </summary>
        public void Method(IEnumerable<byte> enumerable) { }
        /// <summary>
        /// Method(IEnumerable<IEnumerable<int>>)
        /// </summary>
        public void Method(IEnumerable<IEnumerable<int>> enumerable) { }
        /// <summary>
        /// Method(IEnumerable<IEnumerable<byte>>)
        /// </summary>
        public void Method(IEnumerable<IEnumerable<byte>> enumerable) { }
        /// <summary>
        /// Method(IEnumerable<IEnumerable<IEnumerable<int>>>)
        /// </summary>
        public void Method(IEnumerable<IEnumerable<IEnumerable<int>>> enumerable) { }
        /// <summary>
        /// Method(IEnumerable<IEnumerable<IEnumerable<byte>>>)
        /// </summary>
        public void Method(IEnumerable<IEnumerable<IEnumerable<byte>>> enumerable) { }
    }
}
