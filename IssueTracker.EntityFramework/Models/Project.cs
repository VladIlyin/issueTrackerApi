using System;
using System.Collections.Generic;

#nullable disable

namespace IssueTracker.EntityFramework.Models
{
    public partial class Project
    {
        public Project()
        {
            Tasks = new HashSet<Task>();
            UserProjects = new HashSet<UserProject>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
        public virtual ICollection<UserProject> UserProjects { get; set; }
    }
}
