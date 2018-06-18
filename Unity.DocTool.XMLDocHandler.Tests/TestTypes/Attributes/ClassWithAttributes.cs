using DocWorks.Integration.XmlDoc.Tests.TestTypes.GetTypes;
using DocWorks.Integration.XmlDoc.TestUtilities;

namespace DocWorks.Integration.XmlDoc.Tests.TestTypes.Attributes
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
