using System;

namespace DocWorks.Integration.XmlDoc.Tests.TestTypes
{
    public class ClassImplementingGenericInterface : IEquatable<bool>
    {
        public bool Equals(bool other)
        {
            return true;
        }
    }
}
