using AhCh.Users.Entities;

namespace AhCh.Posts.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }


        #region Relations
        public User User { get; set; }
        public ICollection<PostComment> PostComments { get; set; }
        #endregion
    }
}
