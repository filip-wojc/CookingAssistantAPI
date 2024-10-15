namespace CookingAssistantAPI.Exceptions
{
    public class BadRequestException : Exception
    {
        public int Code = 400;
        public BadRequestException(string message) : base(message) { }
    }
}
