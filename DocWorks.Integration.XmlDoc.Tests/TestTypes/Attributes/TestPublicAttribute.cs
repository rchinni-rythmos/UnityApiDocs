using System;
using DocWorks.Integration.XmlDoc.Tests.TestTypes.GetTypes;

namespace DocWorks.Integration.XmlDoc.Tests.TestTypes.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class TestPublicAttribute : Attribute
    {
        public AClass.AnEnum AnEnum { get; set; }
        public string AnString { get; set; }

        public TestPublicAttribute()
        { }
        public TestPublicAttribute(Object value)
        {}
    }
}
