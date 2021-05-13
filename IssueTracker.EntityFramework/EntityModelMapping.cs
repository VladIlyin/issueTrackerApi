using System;
using System.Collections.Generic;
using System.Text;

using IssueTracker.Persistance.Models;
using IssueTracker.EntityFramework.Models;

namespace IssueTracker.EntityFramework
{
    public static class EntityModelMapping
    {
        public static Persistance.Models.User Map(Models.User user)
        {
            return new Persistance.Models.User(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Login,
                user.Password,
                user.RoleId);
        }
    }
}
