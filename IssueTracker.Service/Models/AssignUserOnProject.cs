using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagerApi.Models
{
    public class AssignUserDto
    {
        public Guid ProjectGuid { get; set; }
        public Guid UserGuid { get; set; }
    }
}
