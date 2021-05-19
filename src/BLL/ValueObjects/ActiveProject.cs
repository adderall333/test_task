using System;
using System.Collections.Generic;
using BLL.Entities;
using BLL.Infrastructure;

namespace BLL.ValueObjects
{
    public class ActiveProject : ProjectState
    {
        public DateTime StartTime { get; private set; }

        public ActiveProject(Project project) : base(project)
        {
            StartTime = DateTime.Now;
        }
        
        protected internal override void EndProject()
        {
            Become(new CompletedProject(Project));
        }
        
        protected internal override void DelayProject()
        {
            Become(new NotStartedProject(Project));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return StartTime;
        }
    }
}