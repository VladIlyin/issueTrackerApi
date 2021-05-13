using System.Collections.Generic;
using System.Threading.Tasks;

using IssueTracker.Persistance.Models;

namespace IssueTracker.Persistance.Queries
{
    public interface IUserGetAllQuery
    {
        Task<IEnumerable<User>> ExecuteAsync();
    }
}
