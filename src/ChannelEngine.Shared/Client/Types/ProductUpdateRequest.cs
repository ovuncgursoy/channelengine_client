namespace ChannelEngine.Shared.Client.Types
{
    using System.Text.Json.Serialization;

    public class ProductUpdateRequest
    {
        [JsonPropertyName("MerchantProductNo")]
        public string MerchantProductNo { get; set; }

        [JsonPropertyName("Stock")]
        public int Stock { get; set; }
    }
}
