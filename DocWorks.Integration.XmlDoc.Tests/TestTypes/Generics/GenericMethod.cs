using System.Collections.Generic;
namespace DocWorks.Integration.XmlDoc.Tests.TestTypes.Generics
{
    public class GenericMethod
    {
        /// <summary>
        /// Existing Docs
        /// </summary>
        public List<T> Method<T>()
        {
            return new List<T>();
        }
        /// <summary>
        /// Existing Docs
        /// </summary>
        public void Add<TValue>(string name, int type, IEnumerable<TValue> getter, string metadata = null)
        {
        }
    }
}
