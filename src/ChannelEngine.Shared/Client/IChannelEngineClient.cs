namespace ChannelEngine.Shared.Client
{
    using ChannelEngine.Shared.Client.Types;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>Provides the contract for types that can access ChannelEngine API.</summary>
    public interface IChannelEngineClient
    {
        /// <summary>Fetchs all orders with status IN_PROGRESS from the API.</summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>All orders with status IN_PROGRESS.</returns>
        Task<IEnumerable<Order>> FetchInProgressOrders(CancellationToken cancellationToken = default);

        /// <summary>Updates the stock quantity of the specified product with the specified quantity.</summary>
        /// <param name="productNo">Merchant product no of the product to update.</param>
        /// <param name="quantity">The new stock quantity.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>An instance of <see cref="Result"/> which contains the result of this operation.</returns>
        Task<Result> UpdateProductQuantity(string productNo, int quantity, CancellationToken cancellationToken = default);
    }
}
