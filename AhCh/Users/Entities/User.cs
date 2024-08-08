using AhCh.Posts.Entities;

namespace AhCh.Users.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public bool IsActive { get; set; }
    public DateTime DateOfJoining { get; set; }


    #region Relations
    public ICollection<Post> Posts { get; set; }
    #endregion
}