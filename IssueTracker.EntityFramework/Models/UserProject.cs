using System;
using System.Collections.Generic;

#nullable disable

namespace IssueTracker.EntityFramework.Models
{
    public partial class UserProject
    {
        public int UserProjectId { get; set; }
        public Guid UserId { get; set; }
        public Guid ProjectId { get; set; }

        public virtual Project Project { get; set; }
        public virtual User User { get; set; }
    }
}
