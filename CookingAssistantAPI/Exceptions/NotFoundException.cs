namespace CookingAssistantAPI.Exceptions
{
    public class NotFoundException : Exception
    {
        public int Code = 404;
        public NotFoundException(string message) : base(message) {}
    }
}
