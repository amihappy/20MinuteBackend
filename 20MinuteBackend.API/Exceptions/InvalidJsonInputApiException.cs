namespace _20MinuteBackend.API.Exceptions
{
    public class InvalidJsonInputApiException : ApiException
    {
        public InvalidJsonInputApiException(string errorMessage): base(errorMessage)
        {
        }
    }
}
