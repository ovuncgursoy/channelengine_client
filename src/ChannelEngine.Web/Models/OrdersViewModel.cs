namespace ChannelEngine.Web.Models
{
    using ChannelEngine.Shared.Client.Types;
    using System.Collections.Generic;

    public class OrdersViewModel
    {
        public List<Order> Orders { get; set; }
        public List<Product> Products { get; set; }
    }
}
