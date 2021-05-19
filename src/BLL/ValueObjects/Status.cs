using System.Collections.Generic;
using BLL.Infrastructure;

namespace BLL.ValueObjects
{
    public class Status : ValueObject
    {
        public StatusType Value;

        public static Result<Status> Create(string? value)
        {
            return value switch
            {
                null or "" or "ToDo" or "todo"
                    => new Result<Status>(new Status {Value = StatusType.ToDo}, true),
                "InProgress" or "inprogress"
                    => new Result<Status>(new Status {Value = StatusType.InProgress}, true),
                "Done" or "done"
                    => new Result<Status>(new Status {Value = StatusType.Done}, true),
                _ => new Result<Status>(null, false, "There is no such status type")
            };
        }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
        
        private Status() {}
    }
}