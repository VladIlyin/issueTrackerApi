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
    public class UserController : ControllerBase
    {
        private readonly TaskManagerContext _dbContext;
        private readonly ILogger<UserController> _logger;

        public UserController(
            TaskManagerContext dbContext,
            ILogger<UserController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<UserResponse>> GetAll()
        {
            return await _dbContext
                .Users
                .Select(user => UserResponse.Map(user))
                .ToListAsync();
        }

        [HttpGet("{userGuid}")]
        public async Task<UserResponse> Get([FromRoute] Guid userGuid)
        {
            var user = await _dbContext
                .Users
                .FindAsync(userGuid);

            return UserResponse.Map(user);
        }

        [HttpGet("projects/{userGuid}")]
        public async Task<IActionResult> GetUserProjects(Guid userGuid)
        {
            _logger.LogInformation($"Getting all projects for user with id {userGuid}");

            var userProjects = await _dbContext
                .UserProjects
                .Include("User")
                .Include("Project")
                .Where(x => x.UserId == userGuid)
                .ToListAsync();

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
        public IActionResult Login([FromBody] UserLoginRequest loginInfo)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Login == loginInfo.Login);

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
        public async Task<IActionResult> Add([FromBody] UserAddRequest userDto)
        {
            var user = userDto.ToDal();
            await _dbContext.AddAsync(user);

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
        public async Task<IActionResult> AssignUser([FromBody] UserAssignOnProjectRequest assignUserDto)
        {
            await _dbContext
                    .UserProjects
                    .AddAsync(new UserProject()
                    {
                        ProjectId = assignUserDto.ProjectGuid,
                        UserId = assignUserDto.UserGuid
                    });

            var state = await _dbContext.SaveChangesAsync();

            if (state > 0)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateRequest userDto)
        {
            var user = await _dbContext
                                .Users
                                .FindAsync(userDto.Id);

            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.RoleId = userDto.RoleId;

            _dbContext
                .Users
                .Update(user);

            var state = await _dbContext.SaveChangesAsync();
            return Ok(state);
        }

        [HttpDelete("{userGuid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid userGuid)
        {
            var user = new User()
            {
                Id = userGuid
            };

            _dbContext.Remove(user);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
