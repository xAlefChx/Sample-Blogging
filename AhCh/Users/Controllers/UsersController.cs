using AhCh.Data;
using AhCh.Extensions;
using AhCh.Posts.Entities;
using AhCh.Users.Entities;
using AhCh.Users.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AhCh.Users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AhChContext _context;
        private readonly IConfiguration _configuration;

        public UsersController(AhChContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        #region Users

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var user = new User()
            {
                IsActive = true,
                Username = request.Username,
                Password = PasswordHelper.HashPassword(request.Password),
                DateOfJoining = DateTime.Now,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            return Ok(user.Id);
        }

        [HttpPost("Get-token")]
        public async Task<IActionResult> GetToken([FromBody] GetTokenRequest request, CancellationToken cancellationToken)
        {
            string hashedPassword = PasswordHelper.HashPassword(request.Password);

            User user = await _context.Users.Where(c => c.Username == request.UserName
                                                      && c.Password == hashedPassword)
                                             .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                return BadRequest("Invalid username or password");
            }

            if (user.IsActive == false)
            {
                return BadRequest("User is disabled.");
            }

            string token = JwtHelper.Generate(user!, _configuration);

            return Ok(token);
        }

        [Authorize]
        [HttpDelete("DeleteUsers")]
        public async Task<IActionResult> DeleteUser(CancellationToken cancellationToken)
        {
            int userId = User.Claims.GetUserId();

            User user = await _context.Users.Where(u => u.Id == userId).FirstAsync(cancellationToken);
            List<Comment> comments = await _context.Comments
                                                   .Include(c => c.PostComments)
                                                   .Where(c => c.UserId == userId)
                                                   .ToListAsync(cancellationToken);

            List<PostComment> postComments = comments.SelectMany(c => c.PostComments).ToList();

            _context.PostComments.RemoveRange(postComments);
            _context.Comments.RemoveRange(comments);

            List<Post> posts = await _context.Posts
                                            .Include(c => c.PostComments)
                                            .Where(p => p.UserId == userId)
                                            .ToListAsync(cancellationToken);

            List<PostComment> pc = posts.SelectMany(c => c.PostComments).ToList();
            _context.PostComments.RemoveRange(pc);
            _context.Posts.RemoveRange(posts);

            _context.Users.Remove(user);

            await _context.SaveChangesAsync(cancellationToken);
            return Ok("User Deleted");
        }

        [Authorize]
        [HttpPut("UpdateUsers")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
        {
            int userId = User.Claims.GetUserId();
            User user = await _context.Users.Where(u => u.Id == userId).FirstOrDefaultAsync(cancellationToken);

            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            user!.Username = request.Username;
            user!.Password = PasswordHelper.HashPassword(request.Password);

            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);

            return Ok(user);
        }

        [Authorize]
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
        {
            List<User> users = await _context.Users.ToListAsync(cancellationToken);
            if (users == null)
            {
                return BadRequest("No User Found!");
            }
            return Ok(users);
        }

        [Authorize]
        [HttpPost("GetUserById")]
        public async Task<IActionResult> GetUserById([FromBody] GetUserById request, CancellationToken cancellationToken)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if (user == null)
            {
                return BadRequest("No User Found!");
            }

            return Ok(user);
        }

        #endregion
    }
}