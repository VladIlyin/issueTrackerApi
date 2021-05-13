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
using System.Threading;
using System.Net;

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
            [FromRoute] Guid projectGuid,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext
                .Tasks
                .Include("User")
                .Where(x => x.ProjectId == projectGuid)
                .Select(task => TaskResponse.Map(task))
                .ToListAsync(cancellationToken);
        }

        [HttpGet("{projectGuid}/user/{userGuid}")]
        public async Task<IEnumerable<TaskResponse>> GetProjectUserTasks(
            [FromRoute] Guid projectGuid,
            [FromRoute] Guid userGuid,
            CancellationToken cancellationToken = default)
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
        public async Task<IActionResult> UpdateTask(
            [FromBody] TaskUpdateRequest task,
            CancellationToken cancellationToken = default)
        {
            _dbContext
                .Tasks
                .Update(task.Map());

            var state = await _dbContext.SaveChangesAsync();
            return Ok(state);
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(
            [FromBody] TaskAddRequest task,
            CancellationToken cancellationToken = default)
        {
            var taskDal = task.ToDal();
            await _dbContext
                    .Tasks
                    .AddAsync(task.ToDal());

            try
            {
                await _dbContext.SaveChangesAsync(cancellationToken);
                return Ok(taskDal.Id);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost("statusUpdate")]
        public async Task<IActionResult> UpdateTaskStatus(
            [FromBody] TaskStatusUpdateRequest taskDto,
            CancellationToken cancellationToken = default)
        {
            var task = await _dbContext
                    .Tasks
                    .FindAsync(taskDto.TaskId);

            task.Status = taskDto.Status;
            var state = await _dbContext.SaveChangesAsync(cancellationToken);

            return Ok(state);
        }

        [HttpDelete("{taskGuid}")]
        public async Task<IActionResult> Delete(
            [FromRoute] Guid taskGuid,
            CancellationToken cancellationToken = default)
        {
            var task = await _dbContext
                        .Tasks
                        .FindAsync(new object[] { taskGuid }, cancellationToken);

            if (task == null)
            {
                return NotFound($"Task not found. Id = {taskGuid}");
            }

            _dbContext.Tasks.Remove(task);

            var res = await _dbContext.SaveChangesAsync(cancellationToken);

            return Ok(res);
        }
    }
}
