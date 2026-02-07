using BlogAPI.Data;
using BlogAPI.Models;
using BlogAPI.Models.DTO;
using BlogAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("api/BlogAPI")]
    [Produces("application/json")]
    public class BlogAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly ILogger<BlogAPIController> _logger;
        private readonly IPostRepository _dbPosts;
        public BlogAPIController(ILogger<BlogAPIController> logger, IPostRepository dbPosts)
        {
            _logger = logger;
            _dbPosts = dbPosts;
            this._response = new();
        }

        [Authorize]
        [HttpGet]
        [ResponseCache(Duration = 30)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetPosts([FromQuery] string? tag)
        {
            try
            {
                
                _logger.LogInformation("Getting all posts");

                IEnumerable<PostModel> postList;

                if (tag != null)
                {
                    postList = await _dbPosts.GetAllAsync(u => u.Tag == tag);
                }
                else
                {
                    postList = await _dbPosts.GetAllAsync();
                }
            
                _response.Result = postList;
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Get posts failure: " + ex);
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };

            }
            return _response;
        }

        [Authorize]
        [HttpGet("{id:int}", Name = "GetPost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetPost(int id)
        {
            try
            {
                _logger.LogInformation("Getting post:" + id);
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _logger.LogError("Get post error with Id is 0");
                    return BadRequest(_response);
                }

                var post = await _dbPosts.GetAsync(u => u.Id == id);
                if (post == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _logger.LogError("Get post error Id not found: " + id);
                    return NotFound(_response);
                }
                _response.Result = post;
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Get post failure: " + ex);
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };

            }
            return _response;
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> CreatePost([FromBody] PostCreateDTO postModelDTO)
        {
            try
            {
                _logger.LogInformation("Attempting to create post");
                if (await _dbPosts.GetAsync(u => u.Title.ToLower() == postModelDTO.Title.ToLower()) != null)
                {
                    //ModelState.AddModelError("", "Post Title already exists");
                    _response.StatusCode=HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Post title already exists");
                    return BadRequest(_response);
                }

                if (postModelDTO == null)
                {
                    return BadRequest(postModelDTO);

                }

                PostModel post = new()
                {
                    UserName = postModelDTO.UserName,
                    Title = postModelDTO.Title,
                    Post = postModelDTO.Post,
                    ImageUrl = postModelDTO.ImageUrl,
                    Tag = postModelDTO.Tag,
                    Created_date = DateTimeOffset.Now.ToUniversalTime(),
                    Updated_date = DateTimeOffset.Now.ToUniversalTime()
                };


                await _dbPosts.CreateAsync(post);

                _response.Result = post;
                _response.StatusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;
                return CreatedAtRoute("GetPost", new { id = post.Id }, _response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Create post failure: " + ex);
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };

            }
            return _response;
        }


        [Authorize]
        [HttpDelete("{id:int}", Name = "DeletePost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeletePost(int id)
        {
            try
            {
                _logger.LogInformation("Attempting to delete post:" + id);
                if (id == 0)
                {
                    return BadRequest();
                }

                var post = await _dbPosts.GetAsync(u => u.Id == id);
                if (post == null)
                {
                    return NotFound();
                }

                await _dbPosts.RemoveAsync(post);

                _response.Result = post;
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Delete post failure: " + id + ":" + ex);
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };

            }
            return _response;

        }

        [Authorize]
        [HttpPut("{id:int}", Name = "PutPost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdatePost(int id, [FromBody] PostUpdateDTO postModelDTO)
        {
            try
            {
                _logger.LogInformation("Attempting to update post:");
                if (postModelDTO == null || id != postModelDTO.Id)
                {
                    return BadRequest();
                }


                PostModel post = new()
                {
                    Id = postModelDTO.Id,
                    UserName = postModelDTO.UserName,
                    Title = postModelDTO.Title,
                    Post = postModelDTO.Post,
                    ImageUrl = postModelDTO.ImageUrl,
                    Tag = postModelDTO.Tag,
                   // Created_date = DateTimeOffset.Now.ToUniversalTime(),
                    Updated_date = DateTimeOffset.Now.ToUniversalTime()

                };

                await _dbPosts.UpdateAsync(post);

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Update post failure: " + id + ":" + ex);
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };

            }
            return _response;

        }

    }
}
