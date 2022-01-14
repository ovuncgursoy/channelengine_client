namespace ChannelEngine.Shared.Client
{
    using ChannelEngine.Shared.Client.Types;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class OrderExtensions
    {
        /// <summary>Returns the top N products sold.</summary>
        /// <param name="orders">The list of orders that has the product data.</param>
        /// <param name="top">Number of products to return.</param>
        /// <returns>Top N products sold.</returns>
        public static IEnumerable<Product> TopNProductsSold(this IEnumerable<Order> orders, int top)
        {
            var allOrders = orders ?? throw new ArgumentNullException(nameof(orders));

            var topN = top > 0 ? top : throw new ArgumentException("Can't be a negative value or zero.", nameof(top));

            return allOrders
                .SelectMany (order   => order.Products)
                .GroupBy    (product => new { product.Name, product.ProductNo, product.GTIN })
                .Select     (group   => new Product
                            {
                                Name      = group.Key.Name,
                                ProductNo = group.Key.ProductNo,
                                GTIN      = group.Key.GTIN,
                                Quantity  = group.Aggregate(0, (quantity, product) => quantity + product.Quantity)
                            })
                .OrderByDescending(product => product.Quantity)
                .Take(topN);
        }
    }
}
