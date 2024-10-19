namespace CookingAssistantAPI.Exceptions
{
    public class ForbidException : Exception
    {
        public int Code = 403;
        public ForbidException(string message) : base(message) { }
    }
}
