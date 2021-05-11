using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace TaskManagerApi.Models
{
    public class TaskDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public Guid ProjectId { get; set; }
        public Guid? UserId { get; set; }
        public int? Status { get; set; }
        public UserDto? User { get; set; }

        public static TaskDto ToDto(IssueTracker.EntityFramework.Models.Task task)
        {
            return new TaskDto()
            {
                Id = task.Id,
                Title = task.Title,
                ProjectId = task.ProjectId,
                Status = task.Status,
                UserId = task.UserId,
                User = task.User == null ? null : UserDto.ToDto(task.User)
            };
        }
    }
}
