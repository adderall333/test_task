using BLL.Infrastructure;
using Microsoft.VisualBasic.CompilerServices;

namespace BLL.ValueObjects
{
    public class Priority
    {
        public PriorityType Value { get; private set; }

        public static Result<Priority> Create(string? value)
        {
            return value switch
            {
                null or "" or "Normal" or "normal" 
                    => new Result<Priority>(new Priority {Value = PriorityType.Normal}, true),
                "Minor" or "minor" 
                    => new Result<Priority>(new Priority {Value = PriorityType.Minor}, true),
                "Major" or "major"
                    => new Result<Priority>(new Priority {Value = PriorityType.Major}, true),
                "Critical" or "critical"
                    => new Result<Priority>(new Priority {Value = PriorityType.Critical}, true),
                "ShowStopper" or "showstopper"
                    => new Result<Priority>(new Priority {Value = PriorityType.ShowStopper}, true),
                _ => new Result<Priority>(null, false, "There is no such priority type"),
            };
        }
        
        private Priority() { }
    }
}