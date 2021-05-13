using System;
using System.Collections.Generic;

#nullable disable

namespace IssueTracker.Persistance.Models
{
    public class UserProject
    {
        public UserProject(int userProjectId,
            Guid userId,
            Guid projectId)
        {
            UserProjectId = userProjectId;
            UserId = userId;
            ProjectId = projectId;
        }

        public int UserProjectId { get; }
        public Guid UserId { get; }
        public Guid ProjectId { get; }
    }
}
