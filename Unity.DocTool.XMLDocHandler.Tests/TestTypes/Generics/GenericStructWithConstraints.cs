using System.Collections.Generic;
using DocWorks.Integration.XmlDoc.Tests.TestTypes.Attributes;

namespace DocWorks.Integration.XmlDoc.Tests.TestTypes.Generics
{
    /// <summary>
    /// Existing Docs for GenericStructWithConstraints-T
    /// </summary>
    public struct GenericStructWithConstraints<T> where T : class, IList<DocWorks.Integration.XmlDoc.Tests.TestTypes.GetTypes.AClass>, new()
    {
        /// <summary>
        /// Existing GenericStructWithConstraints-T.GenericMethodWithGenericConstraint-T2
        /// </summary>
        public void GenericMethodWithGenericConstraint<[TestInternal][TestPublic]T2>(T2 t2) where T2 : T
        { }
        /// <summary>
        /// Existing GenericStructWithConstraints-T.GenericMethodWithGenericConstraint
        /// </summary>
        public void GenericMethodWithGenericConstraint()
        { }
    }
}