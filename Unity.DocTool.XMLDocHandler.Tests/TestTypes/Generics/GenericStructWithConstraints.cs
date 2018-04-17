using System.Collections.Generic;

namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes.Generics
{
    /// <summary>
    /// Existing Docs for GenericStructWithConstraints-T
    /// </summary>
    public struct GenericStructWithConstraints<T> where T : class, IList<Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes.AClass>, new()
    {
        /// <summary>
        /// Existing GenericStructWithConstraints-T.GenericMethodWithGenericConstraint-T2
        /// </summary>
        public void GenericMethodWithGenericConstraint<T2>() where T2 : T
        { }
        /// <summary>
        /// Existing GenericStructWithConstraints-T.GenericMethodWithGenericConstraint
        /// </summary>
        public void GenericMethodWithGenericConstraint()
        { }
    }
}