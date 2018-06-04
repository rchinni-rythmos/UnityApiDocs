using System;

namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes
{
    public class ClassImplementingGenericInterface : IEquatable<bool>
    {
        public bool Equals(bool other)
        {
            return true;
        }
    }
}
