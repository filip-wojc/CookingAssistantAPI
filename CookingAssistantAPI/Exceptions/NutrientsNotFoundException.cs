namespace CookingAssistantAPI.Exceptions
{
    public class NutrientsNotFoundException : Exception
    {
        public int Code = 406;
        public NutrientsNotFoundException(string message) : base(message) {}
    }
}
