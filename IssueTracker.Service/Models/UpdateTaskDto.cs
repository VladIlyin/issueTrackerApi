using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagerApi.Models
{
    public class UpdateTaskDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid UserId { get; set; }
        public Guid ProjectId { get; set; }
        public int Status { get; set; }

        public IssueTracker.EntityFramework.Models.Task ToDal()
        {
            return new IssueTracker.EntityFramework.Models.Task()
            {
                Id = Id,
                Title = Title,
                UserId = UserId,
                ProjectId = ProjectId,
                Status = Status
            };
        }
    }
}
