namespace CookingAssistantAPI.Exceptions
{
    public class IngredientsNotFoundException : Exception
    {
        public int Code = 405;
        public IngredientsNotFoundException(string message) : base(message) {}
    }
}
