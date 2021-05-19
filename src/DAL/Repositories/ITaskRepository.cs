using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Entities;
using BLL.Infrastructure;

namespace DAL.Repositories
{
    public interface ITaskRepository
    {
        public Task<Result<IEnumerable<ProjectTask>>> Get();
        public Task<Result<ProjectTask>> Get(int id);
        public Task<Result> Create(string name, int projectId, string priority, string description);
        
        public Task<Result> Delete(int id);
        public Task<Result> UpdateName(int id, string name);
        public Task<Result> UpdatePriority(int id, string priority);
        public Task<Result> UpdateStatus(int id, string status);
        public Task<Result> UpdateDescription(int id, string description);
        
        
    }
}