using System.ComponentModel.DataAnnotations;

namespace AhCh.Posts.Request
{
    public class DeleteCommentRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int PostId { get; set; }
    }
}
