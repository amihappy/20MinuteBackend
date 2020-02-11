namespace _20MinuteBackend.API.Exceptions
{
    public class ResourceNotFoundApiException : ApiException
    {
        public ResourceNotFoundApiException(string resourceName) : base($"Resource {resourceName} not found", System.Net.HttpStatusCode.NotFound)
        {
        }
    }
}
