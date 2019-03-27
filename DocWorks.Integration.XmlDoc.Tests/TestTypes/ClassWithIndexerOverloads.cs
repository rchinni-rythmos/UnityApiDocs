using DocWorks.Integration.XmlDoc.Tests.TestTypes.Attributes;

namespace DocWorks.Integration.XmlDoc.Tests.TestTypes
{
    public class ClassWithIndexerOverloads
    {
        /// <summary>
        /// int indexer
        /// </summary>
        public int this[int a]
        {
            get
            {
                return 1;
            }
            protected set { }
        }
        /// <summary>
        /// string indexer
        /// </summary>
        public int this[string a]
        {
            get
            {
                return 1;
            }
            protected set { }
        }
    }
}
