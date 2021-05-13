using System;
using System.Collections.Generic;
using System.Text;

namespace IssueTracker.Persistance.Models
{
    public class User
    {
        public User(Guid id,
            string firstName,
            string lastName,
            string login,
            string password,
            int? roleId)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Login = login;
            Password = password;
            RoleId = roleId;
        }

        public Guid Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Login { get; }
        public string Password { get; }
        public int? RoleId { get; }
    }
}
