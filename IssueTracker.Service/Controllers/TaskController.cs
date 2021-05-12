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

        /// <summary>
        /// Get project tasks.
        /// </summary>
        /// <param name="projectGuid"></param>
        /// <returns></returns>
        [HttpGet("{projectGuid}", Name = "GetProjectTasks")]
        public async Task<IEnumerable<TaskResponse>> GetProjectTasks(
            [FromRoute] Guid projectGuid)
        {
            return await _dbContext
                .Tasks
                .Include("User")
                .Where(x => x.ProjectId == projectGuid)
                .Select(task => TaskResponse.Map(task))
                .ToListAsync();
        }

        [HttpGet("{projectGuid}/user/{userGuid}")]
        public async Task<IEnumerable<TaskResponse>> GetProjectUserTasks(
            [FromRoute] Guid projectGuid,
            [FromRoute] Guid userGuid)
        {
            return await _dbContext
                .Tasks
                .Include("User")
                .Where(x => x.ProjectId == projectGuid
                        && x.UserId == userGuid)
                .Select(task => TaskResponse.Map(task))
                .ToListAsync();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTask([FromBody] TaskUpdateRequest task)
        {
            _dbContext
                .Tasks
                .Update(task.Map());

            var state = await _dbContext.SaveChangesAsync();
            return Ok(state);
        }

        [HttpPost]
        public async Task<IActionResult> AddTask([FromBody] TaskAddRequest task)
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
        public async Task<IActionResult> UpdateTaskStatus([FromBody] TaskStatusUpdateRequest taskDto)
        {
            var task = await _dbContext
                    .Tasks
                    .FindAsync(taskDto.TaskId);

            task.Status = taskDto.Status;
            var state = await _dbContext.SaveChangesAsync();

            return Ok(state);
        }

        [HttpDelete("{taskGuid}")]
        public async Task<IActionResult> Delete(
            [FromRoute] Guid taskGuid)
        {
            var task = await _dbContext
                        .Tasks
                        .FindAsync(taskGuid);

            _dbContext.Tasks.Remove(task);

            var res = await _dbContext.SaveChangesAsync();

            return Ok(res);
        }
    }
}
