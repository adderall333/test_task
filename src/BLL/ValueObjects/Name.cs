using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BLL.Infrastructure;

namespace BLL.ValueObjects
{
    public class Name : ValueObject
    {
        public string Value { get; private set; }

        public static Result<Name> Create(string? name)
        {
            if (string.IsNullOrEmpty(name))
                return new Result<Name>(null, false, "Project name cannot be empty");

            var projectName = new Name {Value = name};
            return new Result<Name>(projectName, true);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
        
        private Name() {}
    }
}