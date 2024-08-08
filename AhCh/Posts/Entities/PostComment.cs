namespace AhCh.Posts.Entities
{
    public class PostComment
    {
        public int PostId { get; set; }
        public int CommentId { get; set; }


        #region Relations
        public Post Post { get; set; }
        public Comment Comment { get; set; }
        #endregion

    }
}
