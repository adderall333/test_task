using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using BLL.Infrastructure;
using BLL.ValueObjects;

namespace BLL.Entities
{
    public class Project : IEntity
    {
        public int Id { get; private set; }
        
        public Name Name { get; private set; }
        
        public Priority Priority { get; private set; }

        public string StateType { get; set; }

        private ProjectState? _projectState;

        [NotMapped]
        public ProjectState State
        {
            get => _projectState ??= ProjectState.New(StateType, this);
            set => _projectState = value;
        }
        
        public List<ProjectTask> Tasks { get; private set; }

        public static Result<Project> Create(string? name, string? priority = null)
        {
            var project = new Project();

            var nameResult = Name.Create(name);
            if (nameResult.Failure)
                return new Result<Project>(null, false, nameResult.Error);

            project.Name = nameResult.Value;
            
            var priorityResult = Priority.Create(priority);
            if (priorityResult.Failure)
                return new Result<Project>(null, false, priorityResult.Error);

            project.Priority = priorityResult.Value;

            
            project.StateType = nameof(NotStartedProject);
            project.State = new NotStartedProject(project);
            project.Tasks = new List<ProjectTask>();
            
            return new Result<Project>(project, true);
        }

        public Result UpdateName(string name)
        {
            var nameResult = Name.Create(name);
            if (nameResult.Failure)
                return new Result(false, nameResult.Error);

            Name = nameResult.Value;
            return new Result(true);
        }
        
        public Result UpdatePriority(string priority)
        {
            var priorityResult = Priority.Create(priority);
            if (priorityResult.Failure)
                return new Result(false, priorityResult.Error);

            Priority = priorityResult.Value;
            return new Result(true);
        }
        
        // private, parameterless constructor used by EF Core
        private Project() {}
        
        public static readonly Dictionary<string, Func<string, Func<Project, bool>>> Specifications =
            new Dictionary<string, Func<string, Func<Project, bool>>>
            {
                {
                    "NameEqual",
                    name => project => string.Equals(
                        project.Name.Value, 
                        Name.Create(name).Value.Value, 
                        StringComparison.CurrentCultureIgnoreCase)
                },
                {
                    "PriorityGreater",
                    priority => project => project.Priority.Value > Priority.Create(priority).Value.Value
                },
                {
                    "PriorityLess",
                    priority => project => project.Priority.Value < Priority.Create(priority).Value.Value 
                },
                {
                    "PriorityEqual",
                    priority => project => project.Priority.Value == Priority.Create(priority).Value.Value 
                },
                {
                    "StateEqual",
                    state => project => string.Equals(
                        project.State.GetType().Name.Replace("Project", ""), 
                        state, 
                        StringComparison.CurrentCultureIgnoreCase)
                },
            };
    }
}