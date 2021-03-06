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
using IssueTracker.Service.Extensions;

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
            try
            {
                _dbContext
                    .Tasks
                    .Update(task.Map());

                var state = await _dbContext.SaveChangesAsync();
                return Ok(state);
            }
            catch (DbUpdateException ex)
            when (ex.IsPgSqlKeyViolationException())
            {
                return Conflict(ex.InnerException.Message);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(
            [FromBody] TaskAddRequest taskAddRequest,
            CancellationToken cancellationToken = default)
        {
            var taskDal = taskAddRequest.ToDal();

            try
            {
                await _dbContext
                    .Tasks
                    .AddAsync(taskDal, cancellationToken);

                await _dbContext.SaveChangesAsync(cancellationToken);
                return Ok(taskDal.Id);
            }
            catch (DbUpdateException ex)
            when (ex.IsPgSqlKeyViolationException())
            {
                return Conflict(ex.InnerException.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost("statusUpdate")]
        public async Task<IActionResult> UpdateTaskStatus(
            [FromBody] TaskStatusUpdateRequest taskRequest,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var task = await _dbContext
                            .Tasks
                            .FindAsync(taskRequest.TaskId);

                if (task == null)
                {
                    return NotFound($"Task not found. Id = {taskRequest.TaskId}");
                }

                task.Status = taskRequest.Status;
                var state = await _dbContext.SaveChangesAsync(cancellationToken);
                
                return Ok(state);
            }
            catch (DbUpdateException ex)
            when (ex.IsPgSqlKeyViolationException())
            {
                return Conflict(ex.InnerException.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{taskGuid}")]
        public async Task<IActionResult> Delete(
            [FromRoute] Guid taskGuid,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _dbContext.Tasks.Remove(new ProjectTask() { Id = taskGuid });
                var res = await _dbContext.SaveChangesAsync(cancellationToken);

                return Ok(res);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
