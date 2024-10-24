using Microsoft.AspNetCore.Routing.Constraints;

namespace CookingAssistantAPI.DTO.Reviews
{
    public class ReviewGetDTO
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public string? Description { get; set; }
        public string? ReviewAuthor { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
