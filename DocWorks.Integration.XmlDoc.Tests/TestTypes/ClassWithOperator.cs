namespace DocWorks.Integration.XmlDoc.Tests.TestTypes
{
    public class ClassWithOperator
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
