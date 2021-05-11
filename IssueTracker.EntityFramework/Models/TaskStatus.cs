using System;
using System.Collections.Generic;

#nullable disable

namespace IssueTracker.EntityFramework.Models
{
    public partial class TaskStatus
    {
        public TaskStatus()
        {
            Tasks = new HashSet<Task>();
        }

        public int TaskStatusId { get; set; }
        public string StatusName { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
