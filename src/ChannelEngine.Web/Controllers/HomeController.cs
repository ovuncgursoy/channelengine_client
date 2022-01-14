namespace ChannelEngine.Web.Controllers
{
    using ChannelEngine.Shared.Client;
    using ChannelEngine.Web.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    public class HomeController : BaseController
    {
        private readonly IChannelEngineClient channelEngineClient;

        public HomeController(IChannelEngineClient channelEngineClient, ILogger<HomeController> logger) : base(logger) 
            => this.channelEngineClient = channelEngineClient;

        public async Task<IActionResult> Index()
        {
            var orders = await channelEngineClient.FetchInProgressOrders().ConfigureAwait(false);

            return View(new OrdersViewModel()
            {
                Orders = orders.ToList(),
                Products = orders.TopNProductsSold(-5).ToList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Update(string product, int quantity)
        {
            var result = await channelEngineClient.UpdateProductQuantity(product, quantity).ConfigureAwait(false);

            return new JsonResult(new { Message = $"Stock for '{product}' has been updated to {quantity}. ({result.Message.ToLowerInvariant() })" });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
