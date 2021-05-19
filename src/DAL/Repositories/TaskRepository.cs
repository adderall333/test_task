using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Entities;
using BLL.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationContext _context;
        private readonly IProjectRepository _projectRepository;

        public TaskRepository(ApplicationContext context, IProjectRepository projectRepository)
        {
            _context = context;
            _projectRepository = projectRepository;
        }

        public async Task<Result<IEnumerable<ProjectTask>>> Get()
        {
            var tasks = await _context.Tasks.ToListAsync();
            return tasks.Any()
                ? new Result<IEnumerable<ProjectTask>>(tasks, true)
                : new Result<IEnumerable<ProjectTask>>(null, false, "There are no tasks");
        }

        public async Task<Result<ProjectTask>> Get(int id)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            return task is null
                ? new Result<ProjectTask>(null, false, "There is no such task")
                : new Result<ProjectTask>(task, true);
        }

        public async Task<Result> Create(string name, int projectId, string priority, string description)
        {
            var projectResult = await _projectRepository.Get(projectId);
            if (projectResult.Failure)
                return projectResult;
            
            var result = ProjectTask.Create(name, projectResult.Value, priority, description);
            if (result.Failure)
                return result;

            await _context.Tasks.AddAsync(result.Value);
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<Result> Delete(int id)
        {
            var taskResult = await Get(id);
            if (taskResult.Failure)
                return taskResult;

            _context.Tasks.Remove(taskResult.Value);
            await _context.SaveChangesAsync();
            return new Result(true);
        }

        public async Task<Result> UpdateName(int id, string name)
        {
            var taskResult = await Get(id);
            if (taskResult.Failure)
                return taskResult;

            var result = taskResult.Value.UpdateName(name);
            if (result.Failure)
                return result;
            
            await _context.SaveChangesAsync();
            return new Result(true);
        }

        public async Task<Result> UpdatePriority(int id, string priority)
        {
            var taskResult = await Get(id);
            if (taskResult.Failure)
                return taskResult;

            var result = taskResult.Value.UpdatePriority(priority);
            if (result.Failure)
                return result;
            
            await _context.SaveChangesAsync();
            return new Result(true);
        }

        public async Task<Result> UpdateStatus(int id, string status)
        {
            var taskResult = await Get(id);
            if (taskResult.Failure)
                return taskResult;

            var result = taskResult.Value.UpdateStatus(status);
            if (result.Failure)
                return result;
            
            await _context.SaveChangesAsync();
            return new Result(true);
        }

        public async Task<Result> UpdateDescription(int id, string description)
        {
            var taskResult = await Get(id);
            if (taskResult.Failure)
                return taskResult;

            var result = taskResult.Value.UpdateDescription(description);
            if (result.Failure)
                return result;
            
            await _context.SaveChangesAsync();
            return new Result(true);
        }
    }
}