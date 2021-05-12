using System;
using System.Collections.Generic;
using System.Linq;
using IssueTracker.EntityFramework.Models;

namespace TaskManagerApi.Models
{
    public class ProjectResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public static ProjectResponse Map(Project projectDal)
        {
            return new ProjectResponse()
            {
                Id = projectDal.Id,
                Name = projectDal.Name
            };
        }
    }
}
