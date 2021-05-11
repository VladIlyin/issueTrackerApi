using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueTracker.EntityFramework.Models;

namespace TaskManagerApi.Models
{
    public class UserUpdateDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? RoleId { get; set; }

        internal User ToDal()
        {
            return new User()
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName,
                RoleId = RoleId
            };
        }
    }
}
