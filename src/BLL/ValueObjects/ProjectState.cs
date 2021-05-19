using System;
using BLL.Entities;
using BLL.Infrastructure;

namespace BLL.ValueObjects
{
    public abstract class ProjectState : ValueObject
    {
        protected Project Project { get; private set; }
        
        protected ProjectState(Project project)
        {
            Project = project;
        }

        public static ProjectState New(string stateType, Project project)
        {
            return stateType switch
            {
                nameof(NotStartedProject) => new NotStartedProject(project),
                nameof(ActiveProject) => new ActiveProject(project),
                nameof(CompletedProject) => new CompletedProject(project),
                _ => throw new NotSupportedException()
            };
        }

        protected void Become(ProjectState next)
        {
            next.Project = Project;
            Project.State = next;
            Project.StateType = next.GetType().Name;
        }

        protected internal virtual void StartProject() => throw new InvalidOperationException();
        
        protected internal virtual void EndProject() => throw new InvalidOperationException();
        
        protected internal virtual void ReactivateProject() => throw new InvalidOperationException();
        
        protected internal virtual void DelayProject() => throw new InvalidOperationException();
    }
}