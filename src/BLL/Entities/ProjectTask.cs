using System;
using System.Collections.Generic;
using BLL.Infrastructure;
using BLL.ValueObjects;

namespace BLL.Entities
{
    public class ProjectTask : IEntity
    {
        public int Id { get; private set; }
        
        public Name Name { get; private set; }
        
        public Priority Priority { get; private set; }
        
        public Status Status { get; private set; }
        
        public string? Description { get; private set; }
        
        public Project Project { get; private set; }
        public int ProjectId { get; private set; }

        public static Result<ProjectTask> Create(string name, 
            Project project,
            string? priority = null, 
            string? description = null)
        {
            var task = new ProjectTask();
            
            var nameResult = Name.Create(name);
            if (nameResult.Failure)
                return new Result<ProjectTask>(null, false, nameResult.Error);

            task.Name = nameResult.Value;
            
            var priorityResult = Priority.Create(priority);
            if (priorityResult.Failure)
                return new Result<ProjectTask>(null, false, priorityResult.Error);

            task.Priority = priorityResult.Value;
            
            task.Project = project;
            task.ProjectId = project.Id;
            task.Status = Status.Create("ToDo").Value;
            task.Description = description;

            return new Result<ProjectTask>(task, true);
        }

        public Result UpdateName(string? name)
        {
            var nameResult = Name.Create(name);
            if (nameResult.Failure)
                return new Result(false, nameResult.Error);

            Name = nameResult.Value;
            return new Result(true);
        }

        public Result UpdatePriority(string? priority)
        {
            var priorityResult = Priority.Create(priority);
            if (priorityResult.Failure)
                return new Result(false, priorityResult.Error);

            Priority = priorityResult.Value;
            return new Result(true);
        }
        
        public Result UpdateStatus(string? statusType)
        {
            var statusResult = Status.Create(statusType);
            if (statusResult.Failure)
                return new Result(false, statusResult.Error);

            Status = statusResult.Value;
            return new Result(true);
        }
        
        public Result UpdateDescription(string? description)
        {
            Description = description;
            return new Result(true);
        }
        
        // private, parameterless constructor used by EF Core
        private ProjectTask() {}
        
        public static readonly Dictionary<string, Func<string, Func<ProjectTask, bool>>> Specifications =
            new Dictionary<string, Func<string, Func<ProjectTask, bool>>>
            {
                {
                    "NameEqual",
                    name => task => string.Equals(
                        task.Name.Value, 
                        Name.Create(name).Value.Value, 
                        StringComparison.CurrentCultureIgnoreCase)
                },
                {
                    "PriorityGreater",
                    priority => task => task.Priority.Value > Priority.Create(priority).Value.Value
                },
                {
                    "PriorityLess",
                    priority => task => task.Priority.Value < Priority.Create(priority).Value.Value 
                },
            };
    }
}