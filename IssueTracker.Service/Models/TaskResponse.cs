using System;

#nullable enable
namespace TaskManagerApi.Models
{
    public class TaskResponse
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public Guid ProjectId { get; set; }
        public Guid? UserId { get; set; }
        public int? Status { get; set; }
        public UserResponse? User { get; set; }

        public static TaskResponse Map(IssueTracker.EntityFramework.Models.Task task)
        {
            return new TaskResponse()
            {
                Id = task.Id,
                Title = task.Title,
                ProjectId = task.ProjectId,
                Status = task.Status,
                UserId = task.UserId,
                User = task.User == null ? null : UserResponse.Map(task.User)
            };
        }
    }
}
