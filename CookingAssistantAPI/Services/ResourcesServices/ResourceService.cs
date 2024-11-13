using AutoMapper;
using CookingAssistantAPI.DTO.Resources;
using CookingAssistantAPI.Repositories.Ingredients;

namespace CookingAssistantAPI.Services.IngredientServices
{
    public class ResourceService : IResourceService
    {
        private readonly IRepositoryResources _repository;
        private readonly IMapper _mapper;
        public ResourceService(IRepositoryResources repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<CategoriesGetDTO>> GetAllCategoriesAsync()
        {
            var cateogories = await _repository.GetAllCategoriesListAsync();
            return _mapper.Map<List<CategoriesGetDTO>>(cateogories);
        }

        public async Task<List<DifficultiesGetDTO>> GetAllDifficultiesAsync()
        {
            var difficulties = await _repository.GetAllDifficultiesListAsync();
            return _mapper.Map<List<DifficultiesGetDTO>>(difficulties);
        }

        public async Task<List<string>> GetAllIngredientsListAsync()
        {
            return await _repository.GetAllIngredientsListAsync();
        }

        public async Task<List<OccasionsGetDTO>> GetAllOccasionsAsync()
        {
            var occasions = await _repository.GetAllOccasionsListAsync();
            return _mapper.Map<List<OccasionsGetDTO>>(occasions);
        }

        public async Task<List<string>> GetAllUnitsListAsync()
        {
            return await _repository.GetAllUnitsListAsync();
        }
    }
}
