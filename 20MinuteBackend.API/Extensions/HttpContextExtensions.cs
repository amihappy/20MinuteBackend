using System.IO;

namespace _20MinuteBackend.API.Extensions
{
    public static class HttpContextExtensions
    {
        public static async System.Threading.Tasks.Task<string> BodyAsStringAsync(this Microsoft.AspNetCore.Http.HttpContext context)
        {
            using (var reader = new StreamReader(context.Request.Body))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}
