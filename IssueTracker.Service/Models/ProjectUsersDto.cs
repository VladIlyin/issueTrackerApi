using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueTracker.EntityFramework.Models;

namespace TaskManagerApi.Models
{
    public class ProjectUsersDto
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public IEnumerable<UserDto> Users { get; set; }
    }
}
