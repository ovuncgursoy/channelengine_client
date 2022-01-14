namespace ChannelEngine.Shared.Client.Types
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class Order
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("Status")]
        public string Status { get; set; }

        [JsonPropertyName("Lines")]
        public List<Product> Products { get; set; }
    }
}
