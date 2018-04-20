namespace Unity.DocTool.XMLDocHandler.Tests.TestTypes
{
    class ClassWithOperator
    {
        /// <summary>
        /// Plus Operator
        /// </summary>
        public static int operator +(ClassWithOperator classWithOperator, int other)
        {
            return 1;
        }
    }
}
