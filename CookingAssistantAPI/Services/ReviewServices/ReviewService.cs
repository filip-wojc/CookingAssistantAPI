
using AutoMapper;
using CookingAssistantAPI.Database.Models;
using CookingAssistantAPI.DTO.Reviews;
using CookingAssistantAPI.Repositories.Reviews;
using CookingAssistantAPI.Services.UserServices;
using Microsoft.IdentityModel.Tokens;

namespace CookingAssistantAPI.Services.ReviewServices
{
    public class ReviewService : IReviewService
    {
        private readonly IRepositoryReview _repository;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContext;
        public ReviewService(IRepositoryReview repository, IMapper mapper, IUserContextService userContext)
        {
            _mapper = mapper;
            _repository = repository;
            _userContext = userContext;
        }
        
        public async Task<bool> AddReviewAsync(int recipeId, ReviewCreateDTO dto)
        {
            var review = _mapper.Map<Review>(dto);
            review.RatedRecipeId = recipeId;
            review.ReviewAuthorId = _userContext.UserId;

            await _repository.AddReviewAsync(recipeId, review);
            if (await _repository.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> ModifyReviewAsync(int recipeId, ReviewCreateDTO dto)
        {
            var review = _mapper.Map<Review>(dto);
            await _repository.ModifyReviewAsync(recipeId, review);
            if (await _repository.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<ReviewGetDTO> GetUserReview(int recipeId)
        {
            var review = await _repository.GetUserReview(recipeId);

            return _mapper.Map<ReviewGetDTO>(review);
        }
        
        public async Task<List<ReviewGetDTO>> GetReviewsAsync(int recipeId)
        {
            var reviews = await _repository.GetReviewsAsync(recipeId);

            return _mapper.Map<List<ReviewGetDTO>>(reviews);
        }

        public async Task<byte[]> GetProfilePictureAsync(int reviewId)
        {
            var profilePic = await _repository.GetProfilePictureAsync(reviewId);
            return profilePic;
        }
    }
}
