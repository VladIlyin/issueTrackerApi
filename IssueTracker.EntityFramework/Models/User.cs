using System;
using System.Collections.Generic;

#nullable disable

namespace IssueTracker.EntityFramework.Models
{
    public partial class User
    {
        public User()
        {
            Tasks = new HashSet<Task>();
            UserProjects = new HashSet<UserProject>();
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int? RoleId { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
        public virtual ICollection<UserProject> UserProjects { get; set; }
    }
}
