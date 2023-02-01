using System.Text.Json.Serialization;

namespace Business.TransferObjects
{
    public class ErrorResult
	{
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("statusCode")]
        public int StatusCode { get; set; }

        [JsonPropertyName("error")]
        public string Error { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("validation")]
        public ValidationResult  Validation{ get; set; }
    }

    public class ValidationResult
    {
        [JsonPropertyName("source")]
        public string Source { get; set; }

        [JsonPropertyName("keys")]
        public string[] Keys { get; set; }
    }
}
