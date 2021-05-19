using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Entities;
using BLL.Infrastructure;

namespace DAL.Repositories
{
    public interface IProjectRepository
    {
        public Task<Result<IEnumerable<Project>>> Get();
        public Task<Result<Project>> Get(int id);
        public Task<Result> Create(string name, string priority);
        public Task<Result> Delete(int id);
        public Task<Result> UpdateName(int id, string name);
        public Task<Result> UpdatePriority(int id, string priority);
    }
}