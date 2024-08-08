using System.ComponentModel.DataAnnotations;

namespace AhCh.Posts.Request
{
    public class DeletePostRequest
    {
        [Required]
        public int Id { get; set; }
    }
}
