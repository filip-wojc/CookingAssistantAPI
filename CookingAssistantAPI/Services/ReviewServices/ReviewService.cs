
using AutoMapper;
using CookingAssistantAPI.Database.Models;
using CookingAssistantAPI.DTO.Reviews;
using CookingAssistantAPI.Repositories.Reviews;
using CookingAssistantAPI.Services.UserServices;

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
        
        public async Task<bool> AddReviewAsync(ReviewCreateDTO dto, int recipeId)
        {
            var review = _mapper.Map<Review>(dto);
            review.RatedRecipeId = recipeId;
            review.ReviewAuthorId = _userContext.UserId;

            await _repository.AddReviewAsync(review, recipeId);
            if (await _repository.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }
        
    }
}
