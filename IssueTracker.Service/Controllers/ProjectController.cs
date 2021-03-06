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

namespace TaskManagerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly TaskManagerContext _dbContext;
        private readonly ILogger<ProjectController> _logger;

        public ProjectController(
            TaskManagerContext dbContext,
            ILogger<ProjectController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting all projects...");

            var projects = await _dbContext
                .Projects
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Got all projects successfully");
            return Ok(projects.Select(x => ProjectResponse.Map(x)));
        }

        [HttpGet("users/{projectGuid}")]
        public async Task<IActionResult> GetProjectUsers(
            Guid projectGuid,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Getting all users for project with id {projectGuid}");

            var projectUsers = await _dbContext
                .UserProjects
                .Include("User")
                .Include("Project")
                .Where(x => x.ProjectId == projectGuid)
                .ToListAsync(cancellationToken);

            _logger.LogInformation($"Users for project with id {projectGuid} retrived successfully");
            return Ok(
                projectUsers
                    .GroupBy(g => new { g.ProjectId, g.Project.Name })
                    .Select(x => new ProjectUsersResponse()
                    { 
                        ProjectId = x.Key.ProjectId,
                        ProjectName = x.Key.Name,
                        Users = x.Select(pu => UserResponse.Map(pu.User))
                    })
                    .FirstOrDefault());
        }
    }
}
