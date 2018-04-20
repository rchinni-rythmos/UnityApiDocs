using System.Runtime.InteropServices;

namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes
{
    class ClassWithOptionalParameters
    {
        /// <summary>
        /// OptionalInt
        /// </summary>
        public void OptionalInt(int i = 3)
        {}
        /// <summary>
        /// OptionalNoDefaultValue
        /// </summary>
        public void OptionalNoDefaultValue([Optional] string s)
        {}

        private const float optionalConstValue = 4.0f;
        /// <summary>
        /// OptionalConstValue
        /// </summary>
        public void OptionalConstValue(float f = optionalConstValue)
        { }

        public struct AStruct
        {}

        /// <summary>
        /// OptionalDefaultStruct
        /// </summary>
        public void OptionalDefaultStruct(AStruct s = default(AStruct))
        { }
    }
}
