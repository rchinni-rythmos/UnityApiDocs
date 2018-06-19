using System;

namespace DocWorks.Integration.XmlDoc
{
    public class DuplicateMemberException : Exception
    {
        public DuplicateMemberException(string message) : base(message)
        {
        }
    }
}
