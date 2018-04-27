using System;

namespace Unity.DocTool.XMLDocHandler
{
    public class DuplicateMemberException : Exception
    {
        public DuplicateMemberException(string message) : base(message)
        {
        }
    }
}
