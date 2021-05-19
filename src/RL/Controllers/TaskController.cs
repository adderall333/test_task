using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using BLL.Entities;
using BLL.Services;
using DAL;
using DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using RL.Utilities;

namespace RL.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : Controller
    {
        private readonly ITaskRepository _taskRepository;

        public TaskController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectTask>>> Get()
        {
            var result = await _taskRepository.Get();
            return ControllerUtilities.CheckResult(result);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<ProjectTask>> Get(int id)
        {
            var result = await _taskRepository.Get(id);
            return ControllerUtilities.CheckResult(result);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Create(string name, int projectId, string priority, string description)
        {
            var result = await _taskRepository.Create(name, projectId, priority, description);
            return ControllerUtilities.CheckResult(result);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _taskRepository.Delete(id);
            return ControllerUtilities.CheckResult(result);
        }
        
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> UpdateName(int id, string name)
        {
            var result = await _taskRepository.UpdateName(id, name);
            return ControllerUtilities.CheckResult(result);
        }
        
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> UpdatePriority(int id, string priority)
        {
            var result = await _taskRepository.UpdatePriority(id, priority);
            return ControllerUtilities.CheckResult(result);
        }
        
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> UpdateStatus(int id, string status)
        {
            var result = await _taskRepository.UpdateStatus(id, status);
            return ControllerUtilities.CheckResult(result);
        }
        
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> UpdateDescription(int id, string description)
        {
            var result = await _taskRepository.UpdateDescription(id, description);
            return ControllerUtilities.CheckResult(result);
        }
        
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<ProjectTask>>> Filter(string query)
        {
            var tasksResult = await _taskRepository.Get();
            if (tasksResult.Failure)
                return ControllerUtilities.CheckResult(tasksResult);

            try
            {
                var parser = new FilterQueryParser<ProjectTask>(ProjectTask.Specifications);
                var filter = parser.MakeFilter(query);
                return tasksResult
                    .Value
                    .Where(task => filter(task))
                    .ToList();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}