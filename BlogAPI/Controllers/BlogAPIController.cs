using BlogAPI.Data;
using BlogAPI.Models;
using BlogAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("api/BlogAPI")]
    [Produces("application/json")]
    public class BlogAPIController : ControllerBase
    {
        private readonly ILogger<BlogAPIController> _logger;
        private readonly ApplicationDbContext _db;
        public BlogAPIController(ILogger<BlogAPIController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostModelDTO>>> GetPosts()
        {
            _logger.LogInformation("Getting all posts");
            return Ok(await _db.APIPosts.ToListAsync());
        }

        [HttpGet("{id:int}",Name ="GetPost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PostModelDTO>> GetPost(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Get post error with Id " + id);
                return BadRequest();
            }

            var post = await _db.APIPosts.FirstOrDefaultAsync(u => u.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return Ok (post);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PostUpdateDTO>> CreatePost([FromBody]PostCreateDTO postModelDTO)
        {
            if(await _db.APIPosts.FirstOrDefaultAsync(u=> u.Title.ToLower() == postModelDTO.Title.ToLower())!=null)
            {
                ModelState.AddModelError("", "Post Title already exists");
                return BadRequest(ModelState);
            }

            if(postModelDTO == null)
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
                Created_date = DateTime.UtcNow,
                Updated_date = DateTime.UtcNow
            };


            await _db.APIPosts.AddAsync(post);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("GetPost", new {id =post.Id}, post);
        }



        [HttpDelete("{id:int}", Name = "DeletePost")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePost(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }

            var post =await _db.APIPosts.FirstOrDefaultAsync(u => u.Id == id);
            if(post ==null)
            {
                return NotFound();
            }

            _db.APIPosts.Remove(post);
            await _db.SaveChangesAsync();
            return NoContent();

        }

        [HttpPut("{id:int}", Name = "PutPost")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePost(int id, [FromBody]PostUpdateDTO postModelDTO)
        {
            if(postModelDTO == null || id != postModelDTO.Id)
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
                Created_date = DateTime.UtcNow,
                Updated_date = DateTime.UtcNow

            };

            _db.APIPosts.Update(post);
            await _db.SaveChangesAsync();

            return NoContent();

        }

    }
}
