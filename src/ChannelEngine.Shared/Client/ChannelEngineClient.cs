namespace ChannelEngine.Shared.Client
{
    using ChannelEngine.Shared.Client.Types;

    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>Provides access to the ChannelEngine API.</summary>
    public class ChannelEngineClient : IChannelEngineClient
    {
        private readonly HttpClient httpClient;

        /// <summary>Initializes a new instance of the <see cref="ChannelEngineClient"/> type.</summary>
        /// <param name="httpClient">An instance of <see cref="HttpClient"/> that is configured to work with ChannelEngine API.</param>
        public ChannelEngineClient(HttpClient httpClient) => this.httpClient = httpClient;

        /// <summary>Fetchs all orders with status IN_PROGRESS from the API.</summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>All orders with status IN_PROGRESS.</returns>
        public async Task<IEnumerable<Order>> FetchInProgressOrders(CancellationToken cancellationToken = default)
        {
            var endpoint = "orders?statuses=IN_PROGRESS&page=";
            var orders = new List<Order>();
            var page = 1;

            PagedContent<Order> pagedContent;

            do
            {
                var response = await httpClient.GetAsync(endpoint + page, cancellationToken).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

                pagedContent = JsonSerializer.Deserialize<PagedContent<Order>>(json);

                pagedContent.EnsureSuccess();

                orders.AddRange(pagedContent.Content);

                page++;
            } while (pagedContent.Next());

            return orders;
        }

        /// <summary>Updates the stock quantity of the specified product with the specified quantity.</summary>
        /// <param name="productNo">Merchant product no of the product to update.</param>
        /// <param name="quantity">The new stock quantity.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>An instance of <see cref="Result"/> which contains the result of this operation.</returns>
        public async Task<Result> UpdateProductQuantity(string productNo, int quantity, CancellationToken cancellationToken = default)
        {
            var merchangeProductNo =
                !string.IsNullOrWhiteSpace(productNo)
                ? productNo
                : throw new ArgumentException("Can't be null, empty, or can't consist only of whitespace.", nameof(productNo));

            var newQuantity =
                quantity >= 0
                ? quantity
                : throw new ArgumentException("Can't be a negative value.", nameof(quantity));

            var endpoint = "offer";

            var body = new List<ProductUpdateRequest>
                {
                    new ProductUpdateRequest { MerchantProductNo = merchangeProductNo, Stock = newQuantity }
                };

            var response = await httpClient.PutAsJsonAsync(endpoint, body, cancellationToken).ConfigureAwait(false);

            var json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            var result = JsonSerializer.Deserialize<Result>(json);

            result.EnsureSuccess();

            return result;
        }
    }
}
