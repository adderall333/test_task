using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Entities;
using BLL.Services;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using RL.Utilities;

namespace RL.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : Controller
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectController(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> Get()
        {
            var result = await _projectRepository.Get();
            return ControllerUtilities.CheckResult(result);
        }
        
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Project>> Get(int id)
        {
            var result = await _projectRepository.Get(id);
            return ControllerUtilities.CheckResult(result);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Create(string name, string priority)
        {
            var result = await _projectRepository.Create(name, priority);
            return ControllerUtilities.CheckResult(result);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _projectRepository.Delete(id);
            return ControllerUtilities.CheckResult(result);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> UpdateName(int id, string name)
        {
            var result = await _projectRepository.UpdateName(id, name);
            return ControllerUtilities.CheckResult(result);
        }
        
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> UpdatePriority(int id, string priority)
        {
            var result = await _projectRepository.UpdatePriority(id, priority);
            return ControllerUtilities.CheckResult(result);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<Project>>> Filter(string query)
        {
            var projectsResult = await _projectRepository.Get();
            if (projectsResult.Failure)
                return ControllerUtilities.CheckResult(projectsResult);

            try
            {
                var parser = new FilterQueryParser<Project>(Project.Specifications);
                var filter = parser.MakeFilter(query);
                return projectsResult
                    .Value
                    .Where(project => filter(project))
                    .ToList();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}