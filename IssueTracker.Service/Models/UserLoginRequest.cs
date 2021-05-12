using System.ComponentModel.DataAnnotations;

namespace TaskManagerApi.Models
{
    public class UserLoginRequest
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
