using System.ComponentModel.DataAnnotations;

namespace AhCh.Posts.Request
{
    public class UpdatePostRequest
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int Id { get; set; }
    }
}
