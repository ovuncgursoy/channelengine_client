namespace ChannelEngine.Shared.Client.Types
{
    using System.Text.Json.Serialization;

    public class Result
    {
        [JsonPropertyName("StatusCode")]
        public int StatusCode { get; set; }

        [JsonPropertyName("LogId")]
        public object LogId { get; set; }

        [JsonPropertyName("Success")]
        public bool Success { get; set; }

        [JsonPropertyName("Message")]
        public string Message { get; set; }

        public void EnsureSuccess()
        {
            if (!Success)
            {
                throw new ChannelEngineClientException($"Status: {StatusCode}, Message: {Message}, Log Id: {LogId ?? "N/A"}");
            }
        }
    }
}
