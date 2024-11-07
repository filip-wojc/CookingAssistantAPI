using CookingAssistantAPI.DTO.Reviews;
using CookingAssistantAPI.Services;
using CookingAssistantAPI.Services.ReviewServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CookingAssistantAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/reviews")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _service;

        public ReviewController(IReviewService service)
        {
            _service = service;
        }

        [HttpPost("{recipeId}")]
        public async Task<ActionResult> CreateReview([FromRoute] int recipeId, ReviewCreateDTO dto)
        {
            if (await _service.AddReviewAsync(recipeId, dto))
            {
                return Created();
            }
            return BadRequest();
        }

        [HttpPut("{recipeId}/modify")]
        public async Task<ActionResult> ModifyReview([FromRoute] int recipeId, [FromBody] ReviewCreateDTO dto)
        {
            if (await _service.ModifyReviewAsync(recipeId, dto))
            {
                return Created();
            }
            return BadRequest();
        }

        [HttpDelete("{recipeId}/delete")]
        public async Task<ActionResult> DeleteReview([FromRoute] int recipeId)
        {
            if (await _service.DeleteReviewAsync(recipeId))
            {
                return NoContent();
            }
            return BadRequest();
        } 

        [HttpGet("{recipeId}/my-review")]
        public async Task<ActionResult<ReviewGetDTO>> GetUserReview([FromRoute] int recipeId)
        {
            var review = await _service.GetUserReview(recipeId);
            if (review != null)
            {
                return Ok(review);
            }
            return NotFound();
        }

        [HttpGet("{recipeId}")]
        public async Task<ActionResult<List<ReviewGetDTO>>> GetRecipeReviews([FromRoute] int recipeId)
        {
            var reviews = await _service.GetReviewsAsync(recipeId);
            return Ok(reviews);
        }

        [HttpGet("{reviewId}/image")]
        public async Task<ActionResult<byte[]>> GetReviewImage([FromRoute] int reviewId)
        {
            var imageData = await _service.GetProfilePictureAsync(reviewId);
            return File(imageData, "image/jpeg");
        } 
    }
}
