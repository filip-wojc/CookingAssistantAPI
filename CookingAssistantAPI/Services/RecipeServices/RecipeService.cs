using AutoMapper;
using CookingAssistantAPI.Database.Models;
using CookingAssistantAPI.DTO.Recipes;
using CookingAssistantAPI.Exceptions;
using CookingAssistantAPI.Repositories;
using CookingAssistantAPI.Repositories.Recipes;
using CookingAssistantAPI.Services.UserServices;
using CookingAssistantAPI.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;

namespace CookingAssistantAPI.Services.RecipeServices
{
    public class RecipeService : IRecipeService
    {
        private readonly IRepositoryRecipe _repository;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContext;
        public RecipeService(IRepositoryRecipe repository, IMapper mapper, IUserContextService userContext)
        {
            _repository = repository;
            _mapper = mapper;
            _userContext = userContext;
        }
        public async Task<bool> AddRecipe(RecipeCreateDTO recipeDto)
        {
            var recipe = _mapper.Map<Recipe>(recipeDto);
            recipe.CreatedById = _userContext.UserId;

            var ingredientData = recipe.RecipeIngredients
                .Zip(recipeDto.IngredientQuantities, (recipeIngredient, quantity) => new { recipeIngredient, quantity })
                .Zip(recipeDto.IngredientUnits, (pair, unit) => new { pair.recipeIngredient, pair.quantity, unit });

           
            foreach (var data in ingredientData)
            {
                data.recipeIngredient.Quantity = data.quantity;
                data.recipeIngredient.Unit = data.unit;
            }

            recipe.CreatedDate = DateTime.Now;

            await _repository.AddRecipeAsync(recipe);

            return await _repository.SaveChangesAsync() > 0;
        }

        public async Task<bool> ModifyRecipeAsync(RecipeCreateDTO recipeDto, int recipeId)
        {
            var recipe = _mapper.Map<Recipe>(recipeDto);

            var ingredientData = recipe.RecipeIngredients
                .Zip(recipeDto.IngredientQuantities, (recipeIngredient, quantity) => new { recipeIngredient, quantity })
                .Zip(recipeDto.IngredientUnits, (pair, unit) => new { pair.recipeIngredient, pair.quantity, unit });

            foreach (var data in ingredientData)
            {
                data.recipeIngredient.Quantity = data.quantity;
                data.recipeIngredient.Unit = data.unit;
            }

            return await _repository.ModifyRecipeAsync(recipe, recipeId, _userContext.UserId);
        }

        public async Task<bool> DeleteRecipeByIdAsync(int recipeId)
        {
            return await _repository.DeleteRecipeByIdAsync(recipeId, _userContext.UserId);
        }

        public async Task<PageResult<RecipeSimpleGetDTO>> GetAllRecipesAsync(RecipeQuery query)
        {
            var recipesWithNumber = await _repository.GetPaginatedRecipesAsync(query);

            var recipeDtos = _mapper.Map<List<RecipeSimpleGetDTO>>(recipesWithNumber.Item1);

            return new PageResult<RecipeSimpleGetDTO>(recipeDtos, recipesWithNumber.Item2, query.PageSize ?? 10, query.PageNumber ?? 1);

        }

        public async Task<List<RecipeNamesGetDTO>> GetAllRecipesNamesAsync()
        {
            var recipes = await _repository.GetAllRecipesAsync();
            return _mapper.Map<List<RecipeNamesGetDTO>>(recipes);
        }

        public async Task<RecipeGetDTO> GetRecipeByIdAsync(int recipeId)
        {
            var recipe = await _repository.GetRecipeByIdAsync(recipeId);
            return _mapper.Map<RecipeGetDTO>(recipe);
        }

        public async Task<RecipeGetDTO> GetRecipeByNameAsync(string recipeName)
        {
            var recipe = await _repository.GetRecipeByNameAsync(recipeName);
            return _mapper.Map<RecipeGetDTO>(recipe);
        }

        public async Task<RecipeGetDTO> GetRandomRecipeByDate()
        {
            var recipe = await _repository.GetRandomRecipePerDyAsync();
            return _mapper.Map<RecipeGetDTO>(recipe);
        }

        public async Task<byte[]> GetRecipeImageAsync(int recipeId)
        {
            var image = await _repository.GetRecipeImageAsync(recipeId);
            if (image == null)
            {
                throw new BadRequestException("This recipe does not have an image");
            }
            return image;
        }

        public async Task<MemoryStream> GetRecipePdf(int recipeId)
        {
            var recipeDto = await GetRecipeByIdAsync(recipeId);

            var stream = new MemoryStream();
            var document = new PdfDocument();
            var page = document.AddPage();
            var page2 = document.AddPage();
            var graphics = XGraphics.FromPdfPage(page);
            var graphics2 = XGraphics.FromPdfPage(page2);
            XFont font = new XFont("Verdana", 11);
            XFont fontBold = new XFont("Verdana", 13, XFontStyle.Bold);

            graphics.DrawString($"{recipeDto.Name}", fontBold, XBrushes.Black, new XRect(0, 70, page.Width, page.Height), XStringFormats.TopCenter);
            graphics.DrawString($"{recipeDto.AuthorName}", fontBold, XBrushes.Black, new XRect(15, 10, page.Width, page.Height), XStringFormats.TopLeft);
            graphics.DrawString($"Difficulty: {recipeDto.DifficultyName}", fontBold, XBrushes.Black, new XRect(-15, 10, page.Width, page.Height), XStringFormats.TopRight);
            graphics.DrawString($"Serves: {recipeDto.Serves}", fontBold, XBrushes.Black, new XRect(-15, 25, page.Width, page.Height), XStringFormats.TopRight);
            graphics.DrawString($"Time to prepare: {recipeDto.TimeInMinutes}min", fontBold, XBrushes.Black, new XRect(-15, 40, page.Width, page.Height), XStringFormats.TopRight);
            var splittedDesc = SplitIntoEqualParts($"{recipeDto.Description}", 90);
            var lineHeight = 100;
            foreach (var tx in splittedDesc)
            {
                graphics.DrawString(tx, font, XBrushes.Black, new XRect(0, lineHeight, page.Width, page.Height), XStringFormats.TopCenter);
                lineHeight += 15;
            }
            
           
            lineHeight += 15;
            lineHeight = 20;
 
            graphics2.DrawString("How to prepare", fontBold, XBrushes.Black, new XRect(10, lineHeight, page2.Width, page2.Height), XStringFormats.TopLeft);
            lineHeight += 20;
            foreach (var step in recipeDto.Steps)
            {
                graphics2.DrawString($"{step.StepNumber}:", font, XBrushes.Black, new XRect(20, lineHeight, page2.Width, page2.Height), XStringFormats.TopLeft);
                var splittedStep = SplitIntoEqualParts($"{step.Description}", 95);
                foreach (var tx in splittedStep)
                {
                    graphics2.DrawString(tx, font, XBrushes.Black, new XRect(35, lineHeight, page2.Width, page2.Height), XStringFormats.TopLeft);
                    lineHeight += 15;
                }
                lineHeight += 10;
            }
            
            var leftLineHeight = 10;

            for (int i = recipeDto.Ingredients.Count - 1; i >= 0; i--)
            {
                var ing = recipeDto.Ingredients[i];
                graphics.DrawString($"{ing.IngredientName} {ing.Quantity} {ing.Unit}", font, XBrushes.Black, new XRect(10, -leftLineHeight, page.Width, page.Height), XStringFormats.BottomLeft);
                leftLineHeight += 15;
            }
            graphics.DrawString($"Ingredients:", font, XBrushes.Black, new XRect(10, -leftLineHeight, page.Width, page.Height), XStringFormats.BottomLeft);
            
            graphics.DrawString($"Caloricity: {recipeDto.Caloricity} kcal", font, XBrushes.Black, new XRect(-10, -10, page.Width, page.Height), XStringFormats.BottomRight);

            document.Save(stream, false);

            stream.Position = 0;

            return stream;
        }
        private string[] SplitIntoEqualParts(string input, int partLength)
        {
            if (partLength <= 0)
                throw new ArgumentException("Długość części musi być większa niż 0.", nameof(partLength));

            int totalParts = (int)Math.Ceiling((double)input.Length / partLength);
            string[] result = new string[totalParts];

            for (int i = 0; i < totalParts; i++)
            {
                int start = i * partLength;
                result[i] = input.Substring(start, Math.Min(partLength, input.Length - start));
            }

            return result;
        }

      
    }
}
