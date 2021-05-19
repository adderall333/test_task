using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Entities;
using BLL.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationContext _context;

        public ProjectRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<Project>>> Get()
        {
            var projects = await _context.Projects.ToListAsync();
            return projects.Any()
                ? new Result<IEnumerable<Project>>(projects, true)
                : new Result<IEnumerable<Project>>(null, false, "There are no projects");
        }

        public async Task<Result<Project>> Get(int id)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
            return project is null
                ? new Result<Project>(null, false, "There is no such project")
                : new Result<Project>(project, true);
        }

        public async Task<Result> Create(string name, string priority)
        {
            var result = Project.Create(name, priority);
            if (result.Failure)
                return result;

            await _context.Projects.AddAsync(result.Value);
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<Result> Delete(int id)
        {
            var projectResult = await Get(id);
            if (projectResult.Failure)
                return projectResult;

            _context.Projects.Remove(projectResult.Value);
            await _context.SaveChangesAsync();
            return new Result(true);
        }

        public async Task<Result> UpdateName(int id, string name)
        {
            var projectResult = await Get(id);
            if (projectResult.Failure)
                return projectResult;

            var result = projectResult.Value.UpdateName(name);
            if (result.Failure)
                return result;

            await _context.SaveChangesAsync();
            return new Result(true);
        }

        public async Task<Result> UpdatePriority(int id, string priority)
        {
            var projectResult = await Get(id);
            if (projectResult.Failure)
                return projectResult;

            var result = projectResult.Value.UpdatePriority(priority);
            if (result.Failure)
                return result;

            await _context.SaveChangesAsync();
            return new Result(true);
        }
    }
}