using System;
using System.Collections.Generic;

#nullable disable

namespace IssueTracker.Domain.Models
{
    public class Task
    {
        public Task(Guid id,
            string title,
            Guid projectId,
            Guid? userId,
            int? status)
        {
            Id = id;
            Title = title;
            ProjectId = projectId;
            UserId = userId;
            Status = status;
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid ProjectId { get; set; }
        public Guid? UserId { get; set; }
        public int? Status { get; set; }
    }
}
