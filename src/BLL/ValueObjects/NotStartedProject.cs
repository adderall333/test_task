using System.Collections.Generic;
using BLL.Entities;
using BLL.Infrastructure;

namespace BLL.ValueObjects
{
    public class NotStartedProject : ProjectState
    {
        public NotStartedProject(Project project) : base(project)
        {
        }
        
        protected internal override void StartProject()
        {
            Become(new ActiveProject(Project));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield break;
        }
    }
}