using System.ComponentModel.DataAnnotations;

namespace AhCh.Posts.Request
{
    public class SubmitPostRequest
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
