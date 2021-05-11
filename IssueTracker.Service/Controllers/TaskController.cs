using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueTracker.EntityFramework.Models;
using TaskManagerApi.Models;
using ProjectTask = IssueTracker.EntityFramework.Models.Task;

namespace TaskManagerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly TaskManagerContext _dbContext;
        private readonly ILogger<TaskController> _logger;

        public TaskController(
            TaskManagerContext dbContext,
            ILogger<TaskController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTask([FromBody]UpdateTaskDto task)
        {
            _dbContext
                .Tasks
                .Update(task.ToDal());

            var state = await _dbContext.SaveChangesAsync();
            return Ok(state);
        }

        [HttpPost]
        public async Task<IActionResult> AddTask([FromBody] AddTaskDto task)
        {
            var taskDal = task.ToDal();
            await _dbContext
                    .Tasks
                    .AddAsync(task.ToDal());

            var state = await _dbContext.SaveChangesAsync();

            if (state > 0)
            {
                return Ok(taskDal.Id);
            }

            return BadRequest();
        }

        [HttpPost("statusUpdate")]
        public async Task<IActionResult> UpdateTaskStatus([FromBody] UpdateTaskStatusDto taskDto)
        {
            var task = await _dbContext
                    .Tasks
                    .FindAsync(taskDto.TaskId);

            task.Status = taskDto.Status;
            var state = await _dbContext.SaveChangesAsync();

            return Ok(state);
        }

        /// <summary>
        /// Get project tasks.
        /// </summary>
        /// <param name="projectGuid"></param>
        /// <returns></returns>
        [HttpGet("{projectGuid}", Name = "GetProjectTasks")]
        public async Task<IEnumerable<TaskDto>> GetProjectTasks(
            [FromRoute]Guid projectGuid)
        {
            return await _dbContext
                .Tasks
                .Include("User")
                .Where(x => x.ProjectId == projectGuid)
                .Select(task => TaskDto.ToDto(task))
                .ToListAsync();
        }

        [HttpGet("{projectGuid}/user/{userGuid}")]
        public async Task<IEnumerable<TaskDto>> GetProjectUserTasks(
            [FromRoute] Guid projectGuid,
            [FromRoute] Guid userGuid)
        {
            return await _dbContext
                .Tasks
                .Include("User")
                .Where(x => x.ProjectId == projectGuid 
                        && x.UserId == userGuid)
                .Select(task => TaskDto.ToDto(task))
                .ToListAsync();
        }
    }
}
