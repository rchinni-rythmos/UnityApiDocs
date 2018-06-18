using DocWorks.Integration.XmlDoc.TestUtilities;

namespace DocWorks.Integration.XmlDoc.Tests.TestTypes
{
    public class DefaultExternalEnumParameter
    {
        public void Method(ExternalEnum v = ExternalEnum.Value) { }
    }
}
