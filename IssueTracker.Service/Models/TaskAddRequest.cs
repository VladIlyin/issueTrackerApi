using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManagerApi.Models
{
    public class TaskAddRequest
    {
        [Required]
        [MaxLength(320)]
        public string Title { get; set; }
        [Required]
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
