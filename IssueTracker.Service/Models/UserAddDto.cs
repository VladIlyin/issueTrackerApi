using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueTracker.EntityFramework.Models;

namespace TaskManagerApi.Models
{
    public class UserAddDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
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
