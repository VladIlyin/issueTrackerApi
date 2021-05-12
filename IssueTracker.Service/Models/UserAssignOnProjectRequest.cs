using System;

namespace TaskManagerApi.Models
{
    public class UserAssignOnProjectRequest
    {
        public Guid ProjectGuid { get; set; }
        public Guid UserGuid { get; set; }
    }
}
