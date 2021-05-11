using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagerApi.Models
{
    public class UpdateTaskStatusDto
    {
        public Guid TaskId { get; set; }
        public int Status { get; set; }
    }
}
