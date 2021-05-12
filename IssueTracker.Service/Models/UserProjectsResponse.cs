using System;
using System.Collections.Generic;

namespace TaskManagerApi.Models
{
    public class UserProjectsResponse
    {
        public Guid UserId { get; set; }
        public IEnumerable<ProjectResponse> Projects { get; set; }
    }
}
