namespace ChannelEngine.Shared.Client.Types
{
    using System.Text.Json.Serialization;

    public class Product
    {
        [JsonPropertyName("Gtin")]
        public string GTIN { get; set; }

        [JsonPropertyName("MerchantProductNo")]
        public string ProductNo { get; set; }

        [JsonPropertyName("Description")]
        public string Name { get; set; }

        [JsonPropertyName("Quantity")]
        public int Quantity { get; set; }
    }
}
