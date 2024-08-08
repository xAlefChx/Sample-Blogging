using System.ComponentModel.DataAnnotations;

namespace AhCh.Posts.Request
{
    public class SubmitCommentRequest
    {
        [Required]
        public string Content { get; set; }
        [Required]
        public int PostId { get; set; }
    }
}
