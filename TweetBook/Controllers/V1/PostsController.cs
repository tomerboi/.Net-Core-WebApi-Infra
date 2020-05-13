using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBook.Contracts;
using TweetBook.Contracts.V1.Requests;
using TweetBook.Contracts.V1.Response;
using TweetBook.Domain;
using TweetBook.Services;

namespace TweetBook.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostService _postService;
        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet(ApiRoutes.Posts.GetAll)]
        public async Task<IActionResult> Get()
        {
            return Ok(await _postService.GetAllAsync());
        }

        [HttpGet(ApiRoutes.Posts.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            Post ans = await _postService.GetPostByIdAsync(id);
            if (ans == null)
                return NotFound();

            return Ok(ans);
        }

        [HttpPut(ApiRoutes.Posts.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdatePostRequest updatePostRequest)
        {
            var post = new Post
            {
                Id = id,
                Name = updatePostRequest.Name
            };

            var updated = await _postService.UpdatePostAsync(post);
            if (!updated)
                return NotFound();

            return Ok(post);
        }

        [HttpDelete(ApiRoutes.Posts.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deleted = await _postService.DeletePostAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpPost(ApiRoutes.Posts.Create)]
        public async Task<IActionResult> Create([FromBody] CreatePostRequest postRequest)
        {
            var post = new Post { Name = postRequest.Name }; 

            await _postService.CreatePostAsync(post);

            var baseUri = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUri + "/" + ApiRoutes.Posts.Get.Replace("{id}", post.Id.ToString());

            PostResponse response = new PostResponse { Id = post.Id };
            return Created(locationUrl, response);
        }
    }
}
