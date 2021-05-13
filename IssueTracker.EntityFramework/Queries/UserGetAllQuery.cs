using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using IssueTracker.EntityFramework.Models;
using IssueTracker.Persistance.Queries;

namespace IssueTracker.EntityFramework.Queries
{
    public class UserGetAllQuery : IUserGetAllQuery
    {
        private readonly TaskManagerContext _dbContext;

        public UserGetAllQuery(TaskManagerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Persistance.Models.User>> ExecuteAsync()
        {
            return await _dbContext
                .Users
                .Select(user => EntityModelMapping.Map(user))
                .ToListAsync();
        }
    }
}
