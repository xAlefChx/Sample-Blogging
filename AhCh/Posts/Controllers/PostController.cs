using AhCh.Data;
using AhCh.Extensions;
using AhCh.Posts.Entities;
using AhCh.Posts.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AhCh.Posts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly AhChContext _context;

        public PostController(AhChContext context)
        {
            _context = context;
        }

        #region Posts

        [Authorize]
        [HttpPost("SubmitPosts")]
        public async Task<IActionResult> SubmitPost([FromBody] SubmitPostRequest request, CancellationToken cancellationToken)
        {
            int userId = User.Claims.GetUserId();

            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var post = new Post()
            {
                Content = request.Content,
                Title = request.Title,
                UserId = userId
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync(cancellationToken);

            return Ok(post);
        }

        [Authorize]
        [HttpDelete("DeletePosts")]
        public async Task<IActionResult> DeletePost([FromBody] DeletePostRequest request, CancellationToken cancellationToken)
        {
            int userId = User.Claims.GetUserId();

            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            Post post = await _context.Posts
                                       .Include(c => c.PostComments)
                                       .ThenInclude(c => c.Comment)
                                       .FirstOrDefaultAsync(p => p.Id == request.Id && p.UserId == userId, cancellationToken);
            if (post == null)
            {
                return BadRequest("Invalid Post");
            }

            List<Comment> comments = post.PostComments.Select(c => c.Comment).ToList();

            _context.PostComments.RemoveRange(post.PostComments);
            _context.Comments.RemoveRange(comments);
            _context.Posts.Remove(post);

            await _context.SaveChangesAsync(cancellationToken);

            return Ok();
        }

        [Authorize]
        [HttpPut("UpdatePosts")]
        public async Task<IActionResult> UpdatePost([FromBody] UpdatePostRequest request, CancellationToken cancellationToken)
        {
            int userId = User.Claims.GetUserId();

            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            Post post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == request.Id && p.UserId == userId, cancellationToken);
            if (post == null)
            {
                return BadRequest("Invalid Post");
            }

            _context.Entry(post).State = EntityState.Modified;

            await _context.SaveChangesAsync(cancellationToken);

            return Ok(post);
        }

        [Authorize]
        [HttpGet("GetAllPosts")]
        public async Task<IActionResult> GetAllPosts(CancellationToken cancellationToken)
        {
            List<Post> posts = await _context.Posts.ToListAsync(cancellationToken);
            if (posts == null)
            {
                return BadRequest("No Post Found!");
            }
            return Ok(posts);
        }

        [Authorize]
        [HttpPost("GetPostById")]
        public async Task<IActionResult> GetPostById([FromBody] GetPostByIdRequest request, CancellationToken cancellationToken)
        {
            Post post = await _context.Posts.Where(p => p.Id == request.PostId).FirstOrDefaultAsync(cancellationToken);
            if (post == null)
            {
                return BadRequest("No Post Found!");
            }
            return Ok(post);
        }

        #endregion

        #region Comments
        [Authorize]
        [HttpPost("SubmitComments")]
        public async Task<IActionResult> SubmitComment([FromBody] SubmitCommentRequest request, CancellationToken cancellationToken)
        {
            int userId = User.Claims.GetUserId();

            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var comment = new Comment()
            {
                UserId = userId,
                Content = request.Content,
                PostComments = new List<PostComment>()
                {
                    new PostComment()
                    {
                         PostId = request.PostId
                    }
                }
            };

            _context.Comments.Add(comment);

            await _context.SaveChangesAsync(cancellationToken);

            return Ok();
        }

        [Authorize]
        [HttpDelete("DeleteComments")]
        public async Task<IActionResult> DeleteComment([FromBody] DeleteCommentRequest request, CancellationToken cancellationToken)
        {
            int userId = User.Claims.GetUserId();

            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            Comment comment = await _context.Comments
                                             .Include(c => c.PostComments)
                                             .FirstOrDefaultAsync(c => c.Id == request.Id && c.UserId == userId, cancellationToken);
            if (comment == null)
            {
                return BadRequest("Invalid comment");
            }

            PostComment postComment = comment.PostComments.Where(c => c.CommentId == comment.Id && c.PostId == request.PostId).First();


            _context.PostComments.Remove(postComment);
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync(cancellationToken);

            return Ok();
        }

        [Authorize]
        [HttpGet("GetAllComments")]
        public async Task<IActionResult> GetAllComments(CancellationToken cancellationToken)
        {
            List<Comment> Comment = await _context.Comments.ToListAsync(cancellationToken);
            if (Comment == null)
            {
                return BadRequest("No Comment Found!");
            }
            return Ok(Comment);
        }

        [Authorize]
        [HttpPost("GetCommentById")]
        public async Task<IActionResult> GetCommentById([FromBody] GetCommentByIdRequest request, CancellationToken cancellationToken)
        {
            Comment Comment = await _context.Comments.Where(c => c.Id == request.Id).FirstOrDefaultAsync(cancellationToken);
            if (Comment == null)
            {
                return BadRequest("No Comment Found!");
            }
            return Ok(Comment);
        }

        #endregion
    }
}