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
using IssueTracker.Persistance.Queries;
using System.Threading;
using System.Net;

namespace TaskManagerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly TaskManagerContext _dbContext;
        private readonly IUserGetAllQuery _userGetAllQuery;
        private readonly ILogger<UserController> _logger;

        public UserController(
            TaskManagerContext dbContext,
            IUserGetAllQuery userGetAllQuery,
            ILogger<UserController> logger)
        {
            _dbContext = dbContext;
            _userGetAllQuery = userGetAllQuery;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<IssueTracker.Persistance.Models.User>> GetAll(
            CancellationToken cancellationToken = default)
        {
            return await _userGetAllQuery.ExecuteAsync();
        }

        [HttpGet("{userGuid}")]
        public async Task<UserResponse> Get(
            [FromRoute] Guid userGuid,
            CancellationToken cancellationToken = default)
        {
            var user = await _dbContext
                .Users
                .FindAsync(new object[] { userGuid }, cancellationToken);

            return UserResponse.Map(user);
        }

        [HttpGet("projects/{userGuid}")]
        public async Task<IActionResult> GetUserProjects(
            Guid userGuid,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Getting all projects for user with id {userGuid}");

            var userProjects = await _dbContext
                .UserProjects
                .Include("User")
                .Include("Project")
                .Where(x => x.UserId == userGuid)
                .ToListAsync(cancellationToken);

            _logger.LogInformation($"Projects for user with id {userGuid} retrived successfully");
            return Ok(
                userProjects
                    .GroupBy(g => new { g.UserId })
                    .Select(x => new UserProjectsResponse()
                    {
                        UserId = x.Key.UserId,
                        Projects = x.Select(pu => ProjectResponse.Map(pu.Project))
                    })
                    .FirstOrDefault());
        }

        [HttpPost("login")]
        public IActionResult Login(
            [FromBody] UserLoginRequest loginInfo,
            CancellationToken cancellationToken = default)
        {
            var user = _dbContext
                .Users
                .Where(x => x.Login == loginInfo.Login)
                .FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            if (user.Password != loginInfo.Password)
            {
                return Unauthorized();
            }

            return Ok(user);
        }

        /// <summary>
        /// Create user.
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add(
            [FromBody] UserAddRequest userDto,
            CancellationToken cancellationToken = default)
        {
            var user = userDto.ToDal();
            await _dbContext
                .AddAsync(user, cancellationToken);

            var state = await _dbContext.SaveChangesAsync();

            if (state > 0)
            {
                return Ok(user.Id);
            }

            return BadRequest();
        }

        /// <summary>
        /// Assign user on project.
        /// </summary>
        /// <param name="assignUserDto"></param>
        /// <returns></returns>
        [HttpPost("assign")]
        public async Task<IActionResult> AssignUser(
            [FromBody] UserAssignOnProjectRequest assignUserDto,
            CancellationToken cancellationToken = default)
        {
            await _dbContext
                    .UserProjects
                    .AddAsync(new UserProject()
                    {
                        ProjectId = assignUserDto.ProjectGuid,
                        UserId = assignUserDto.UserGuid
                    }, cancellationToken);

            var state = await _dbContext.SaveChangesAsync();

            if (state > 0)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(
            [FromBody] UserUpdateRequest userUpdateRequest,
            CancellationToken cancellationToken = default)
        {
            //TODO: try patch instead put, cause we update certain fields here
            try
            {
                var user = await _dbContext
                    .Users
                    .FindAsync(new object[] { userUpdateRequest.Id }, cancellationToken);

                if (user == null)
                {
                    return NotFound();
                }

                user.FirstName = userUpdateRequest.FirstName;
                user.LastName = userUpdateRequest.LastName;
                user.RoleId = userUpdateRequest.RoleId;

                _dbContext
                    .Users
                    .Update(user);

                var state = await _dbContext.SaveChangesAsync();
                return Ok(state);
            }
            catch (DbUpdateException ex)
            when (ex.InnerException is Npgsql.PostgresException
                && (ex.InnerException as Npgsql.PostgresException).SqlState == Npgsql.PostgresErrorCodes.ForeignKeyViolation)
            {
                return Conflict(ex.InnerException.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{userGuid}")]
        public async Task<IActionResult> Delete(
            [FromRoute] Guid userGuid,
            CancellationToken cancellationToken = default)
        {
            var user = new User()
            {
                Id = userGuid
            };

            _dbContext.Remove(user);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Ok();
        }
    }
}
