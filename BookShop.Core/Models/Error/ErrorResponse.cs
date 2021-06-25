using Newtonsoft.Json;

namespace BookShop.Core.Models.Error
{
    public class ErrorResponse
    {
        public ErrorResponse(string exceptionName, string message)
        {
            ExceptionName = exceptionName;
            Message = message;
        }

        public string ExceptionName { get; }
        public string Message { get; }
    
        public string ToJson()
        {
            return JsonConvert.SerializeObject(new { ExceptionName, Message });
        }
    }
}
