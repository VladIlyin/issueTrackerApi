using System;
using System.ComponentModel.DataAnnotations;

using IssueTracker.EntityFramework.Models;

namespace TaskManagerApi.Models
{
    public class UserUpdateRequest
    {
        [Required]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? RoleId { get; set; }

        internal User Map()
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
