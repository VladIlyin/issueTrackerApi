using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManagerApi.Models
{
    public class TaskStatusUpdateRequest
    {
        [Required]
        public Guid TaskId { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Status { get; set; }
    }
}
