using System;
using Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes;

namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class TestPublicAttribute : Attribute
    {
        public AClass.AnEnum AnEnum { get; set; }
        public TestPublicAttribute()
        { }
        public TestPublicAttribute(Object value)
        {}
    }
}
