using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueTracker.EntityFramework.Models;

namespace TaskManagerApi.Models
{
    public class UserProjectsDto
    {
        public Guid UserId { get; set; }
        public IEnumerable<ProjectDto> Projects { get; set; }
    }
}
