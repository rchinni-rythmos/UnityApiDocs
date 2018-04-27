using Unity.DocTool.XMLDocHandler.TestUtilities;

namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes
{
    public class DefaultExternalEnumParameter
    {
        public void Method(ExternalEnum v = ExternalEnum.Value) { }
    }
}
