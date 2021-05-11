using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagerApi.Models
{
    public class AddTaskDto
    {
        public string Title { get; set; }
        public Guid ProjectId { get; set; }
        public Guid? UserId { get; set; }
        public int? Status { get; set; }

        public IssueTracker.EntityFramework.Models.Task ToDal()
        {
            return new IssueTracker.EntityFramework.Models.Task()
            {
                Id = Guid.NewGuid(),
                Title = Title,
                ProjectId = ProjectId,
                UserId = UserId,
                Status = Status
            };
        }
    }
}
