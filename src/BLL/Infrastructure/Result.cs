using System;

namespace BLL.Infrastructure
{
    public class Result
    {
        public bool Success { get; private set; }
        public string? Error { get; private set; }

        public bool Failure => !Success;

        public Result(bool success, string? error = null)
        {
            Contracts.Require(success || !string.IsNullOrEmpty(error), "You cant create result this way");
            Contracts.Require(!success || string.IsNullOrEmpty(error), "You cant create result this way");

            Success = success;
            Error = error;
        }
    }
    
    public class Result<T> : Result
    {
        private readonly T? _value;

        public T Value
        {
            get
            {
                Contracts.Require(Success, "Result is unsuccessful");

                return _value!;
            }
        }

        public Result(T? value, bool success, string? error = null)
            : base(success, error)
        {
            Contracts.Require(value != null || !success, "You cant create result this way");

            _value = value;
        }
    }

    public static class Contracts
    {
        public static void Require(bool precondition, string exceptionMessage)
        {
            if (!precondition)
                throw new CodeContractException(exceptionMessage);
        }
    }
}