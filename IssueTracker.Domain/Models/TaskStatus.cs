using System;
using System.Collections.Generic;

#nullable disable

namespace IssueTracker.Domain.Models
{
    public class TaskStatus
    {
        public TaskStatus(int taskStatusId, string statusName)
        {
            TaskStatusId = taskStatusId;
            StatusName = statusName;
        }

        public int TaskStatusId { get; }
        public string StatusName { get; }
    }
}
