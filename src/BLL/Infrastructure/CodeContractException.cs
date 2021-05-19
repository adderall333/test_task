using System;

namespace BLL.Infrastructure
{
    public class CodeContractException : Exception
    {
        public CodeContractException(string message) : base(message) { }
    }
}