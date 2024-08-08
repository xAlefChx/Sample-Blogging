using System.ComponentModel.DataAnnotations;

namespace AhCh.Users.Requests
{
    public class RegisterRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}