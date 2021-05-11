using System;
using System.Collections.Generic;

#nullable disable

namespace IssueTracker.EntityFramework.Models
{
    public partial class Task
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid ProjectId { get; set; }
        public Guid? UserId { get; set; }
        public int? Status { get; set; }

        public virtual Project Project { get; set; }
        public virtual TaskStatus StatusNavigation { get; set; }
        public virtual User User { get; set; }
    }
}
