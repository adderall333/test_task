using System;
using System.Collections.Generic;
using BLL.Entities;
using BLL.Infrastructure;

namespace BLL.ValueObjects
{
    public class CompletedProject : ProjectState
    {
        public DateTime StartTime { get; private set; }
        
        public DateTime EndTime { get; private set; }

        public CompletedProject(Project project) : base(project)
        {
            StartTime = (project.State as ActiveProject)!.StartTime;
            EndTime = DateTime.Now;
        }
        
        protected internal override void ReactivateProject()
        {
            Become(new ActiveProject(Project));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return StartTime;
            yield return EndTime;
        }
    }
}