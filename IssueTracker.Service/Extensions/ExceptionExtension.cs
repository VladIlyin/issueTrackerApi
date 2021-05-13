using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IssueTracker.Service.Extensions
{
    public static class ExceptionExtension
    {
        public static bool IsPgSqlKeyViolationException(this Exception ex)
        {
            if (ex.InnerException is Npgsql.PostgresException
                && (ex.InnerException as Npgsql.PostgresException)
                    .SqlState == Npgsql.PostgresErrorCodes.ForeignKeyViolation)
            {
                return true;
            }

            return false;
        }
    }
}
