namespace ChannelEngine.Shared.Client
{
    public class ChannelEngineConfiguration
    {
        public string BaseUrl { get; set; }
        public string ApiKey { get; set; } // this should come from a key vault
    }
}
