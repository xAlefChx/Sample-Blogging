using System.ComponentModel.DataAnnotations;

namespace AhCh.Posts.Request
{
    public class GetPostByIdRequest
    {
        [Required]
        public int PostId { get; set; }
    }
}
