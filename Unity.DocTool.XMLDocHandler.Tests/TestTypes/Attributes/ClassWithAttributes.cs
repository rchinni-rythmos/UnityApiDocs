﻿using Unity.DocTool.XMLDocHandler.Tests.TestTypes.GetTypes;
using Unity.DocTool.XMLDocHandler.TestUtilities;

namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes.Attributes
{
    [TestInternal] [TestPublic(50, AnEnum = AClass.AnEnum.Value)]
    public class ClassWithAttributes
    {
        public void MethodWithParameter([TestInternal][TestPublic]int i)
        { }

        [return: TestInternal]
        [return: TestPublic]
        public int MethodWithReturn()
        {
            return 1;
        }
    }
}
