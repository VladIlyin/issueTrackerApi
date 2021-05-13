using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManagerApi.Models
{
    public class TaskUpdateRequest
    {
        [Required]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid? UserId { get; set; }
        public Guid ProjectId { get; set; }
        public int Status { get; set; }

        public IssueTracker.EntityFramework.Models.Task Map()
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
