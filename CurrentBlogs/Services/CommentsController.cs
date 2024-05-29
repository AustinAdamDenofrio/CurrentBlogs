using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using CurrentBlogs.Client.Components.Services.Interfaces;
using CurrentBlogs.Data;
using Microsoft.AspNetCore.Authorization;
using CurrentBlogs.Client.Components.Models;

namespace CurrentBlogs.Controllers
{
    [Route("api/[controller]")] //api/comments
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsDTOService _commentsService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentsController(ICommentsDTOService commentService, UserManager<ApplicationUser> userManager)
        {
            _commentsService = commentService;
            _userManager = userManager;
        }

        // get comments by blogpost id - anyone
        // we figure out route
        [HttpGet("updated/{blogPostId:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CommentDTO>>> GetCommentsByBlogPostIdAsync([FromRoute] int blogPostId)
        {
            try
            {
                IEnumerable<CommentDTO?> comments = await _commentsService.GetCommentsByBlogPostIdAsync(blogPostId);

                if (comments == null)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(comments);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }

        // get comment by ID - anyone
        [HttpGet("{commentId:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<CommentDTO>> GetCommentByIdAsync([FromRoute] int commentId)
        {
            try
            {
                CommentDTO? comment = await _commentsService.GetCommentByIdAsync(commentId);

                if (comment == null)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(comment);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }

        //create new comment - logging in
        [HttpPost("create")]
        public async Task<IActionResult> CreateComment([FromBody] CommentDTO comment)
        {
            string userId = _userManager.GetUserId(User)!;

            try
            {
                await _commentsService.CreateCommentAsync(comment, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Problem();
            }
        }

        //update a comment - logged in (AND comment belongs to user or admin or moderator)
        [HttpPut("{commentId:int}")]
        public async Task<IActionResult> UpdateComment([FromBody] CommentDTO comment, [FromRoute] int commentId)
        {
            if (comment.Id != commentId)
            {
                return BadRequest();
            }

            string userId = _userManager.GetUserId(User)!;
            bool inAuthorRole = User.IsInRole("Author");
            bool inModeratorRole = User.IsInRole("Moderator");

            CommentDTO? commentDTO = await _commentsService.GetCommentByIdAsync(commentId);

            if (commentDTO is not null)
            {
                if (inAuthorRole || inModeratorRole || commentDTO.AuthorId == userId)
                {
                    await _commentsService.UpdateCommentAsync(comment, userId);
                    return Ok();
                }
            }
            return NotFound();
        }





        //delete a comment - logged in (AND comment belings to user or admin or moderator)
        [HttpDelete("{commentId:int}")] // Delete: api/comments/5
        [Authorize]
        public async Task<IActionResult> DeleteComment([FromRoute] int commentId)
        {
            string userId = _userManager.GetUserId(User)!;
            bool inAuthorRole = User.IsInRole("Author");
            bool inModeratorRole = User.IsInRole("Moderator");

            CommentDTO? comment = await _commentsService.GetCommentByIdAsync(commentId);

            if (inAuthorRole || inModeratorRole || comment?.AuthorId == userId)
            {
                await _commentsService.DeleteCommentAsync(commentId);
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }


    }
}
