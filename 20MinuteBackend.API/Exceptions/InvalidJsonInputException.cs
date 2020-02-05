namespace _20MinuteBackend.API.Exceptions
{
    public class InvalidJsonInputException : ApiException
    {
        public InvalidJsonInputException(string errorMessage): base(errorMessage)
        {
        }
    }
}
