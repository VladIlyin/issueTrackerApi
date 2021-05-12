using System;
using System.ComponentModel.DataAnnotations;

using IssueTracker.EntityFramework.Models;

namespace TaskManagerApi.Models
{
    public class UserAddRequest
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Login { get; set; }
        public string Password { get; set; }
        public int? RoleId { get; set; }

        internal User ToDal()
        {
            return new User()
            {
                Id = Guid.NewGuid(),
                FirstName = FirstName,
                LastName = LastName,
                Login = Login,
                Password = Password,
                RoleId = RoleId
            };
        }
    }
}
