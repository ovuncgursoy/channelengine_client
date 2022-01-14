namespace ChannelEngine.CLI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using ChannelEngine.Shared.Client;
    using ChannelEngine.Shared.Client.Types;
    using ConsoleTables;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    internal static class Program
    {
        internal static async Task Main()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var provider = SetupDI(config);

            var channelEngineClient = provider.GetRequiredService<IChannelEngineClient>();

            var orders = await FetchOrders(channelEngineClient).ConfigureAwait(false);
            
            ListProducts(orders);
            
            await UpdateStock(channelEngineClient).ConfigureAwait(false);

            Console.Write($"{Environment.NewLine} Press any key to exit.");
            Console.ReadKey();
        }

        private static async Task UpdateStock(IChannelEngineClient channelEngineClient)
        {
            Console.Write($"{Environment.NewLine} Please enter the product no: ");
            var productNo = Console.ReadLine();

            // validate productNo

            Console.Write($"{Environment.NewLine} Please enter the new stock quantity: ");
            var quantity = Int32.Parse(Console.ReadLine());

            // validate quantity

            var result = await channelEngineClient.UpdateProductQuantity(productNo, quantity).ConfigureAwait(false);

            Console.Write($"{Environment.NewLine} Product stock updated ({result.Message})");
        }

        private static void ListProducts(IEnumerable<Order> orders)
        {
            var products = orders.TopNProductsSold(5);
            Console.WriteLine($"{Environment.NewLine} Top 5 most sold products");

            ConsoleTable
                .From<Product>(products)
                .Configure(output => output.NumberAlignment = Alignment.Right)
                .Write();
        }

        private static async Task<List<Order>> FetchOrders(IChannelEngineClient channelEngineClient)
        {
            Console.WriteLine("Fetching all products with status IN_PROGRESS");
            var enumerable = await channelEngineClient.FetchInProgressOrders().ConfigureAwait(false);
            var orders = enumerable.ToList();

            Console.WriteLine($"{Environment.NewLine} Found {orders.Count} order(s).");

            return orders;
        }

        private static ServiceProvider SetupDI(IConfigurationRoot configuration) =>
            new ServiceCollection()
            .AddLogging()
            .AddSingleton<IChannelEngineClient, ChannelEngineClient>(provider =>
            {
                var channelEngineConfig = configuration.GetRequiredSection("ChannelEngineConfiguration").Get<ChannelEngineConfiguration>();

                var httpClient = new HttpClient() { BaseAddress = new Uri(channelEngineConfig.BaseUrl) };
                httpClient.DefaultRequestHeaders.Add("X-CE-KEY", channelEngineConfig.ApiKey);

                return new ChannelEngineClient(httpClient);
            })
            .BuildServiceProvider();
    }
}
