using System;
using System.Collections.Generic;

namespace TaskManagerApi.Models
{
    public class ProjectUsersResponse
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public IEnumerable<UserResponse> Users { get; set; }
    }
}
