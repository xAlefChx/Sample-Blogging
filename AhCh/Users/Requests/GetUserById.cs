using System.ComponentModel.DataAnnotations;

namespace AhCh.Users.Requests
{
    public class GetUserById
    {
        [Required]
        public int Id { get; set; }
    }
}