using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueTracker.EntityFramework.Models;

namespace TaskManagerApi.Models
{
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public static ProjectDto ToDto(Project projectDal)
        {
            return new ProjectDto()
            {
                Id = projectDal.Id,
                Name = projectDal.Name
            };
        }
    }
}
