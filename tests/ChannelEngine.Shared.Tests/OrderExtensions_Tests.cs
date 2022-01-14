namespace ChannelEngine.Shared.Tests
{
    using ChannelEngine.Shared.Client;
    using ChannelEngine.Shared.Client.Types;
    using NUnit.Framework;
    using Shouldly;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.Json;

    [TestFixture]
    public static class OrderExtensions_Tests
    {
        [Test]
        public static void TopNProductsSold_when_called_with_a_null_orders_parameter_should_throw_ArgumentNullException()
        {
            // arrange
            IEnumerable<Order> orders = null;
            var arbitraryTop = 1;

            // act
            Should.Throw<ArgumentNullException>(() => orders.TopNProductsSold(arbitraryTop));

            // assert
        }

        [TestCase(0), TestCase(-1), TestCase(-42)]
        public static void TopNProductsSold_when_called_with_a_negative_or_zero_top_value_should_throw_ArgumentException(int top)
        {
            // arrange
            var orders = OrderData();

            // act
            var exception = Should.Throw<ArgumentException>(() => orders.TopNProductsSold(top));

            // assert
            exception.Message.ShouldBe("Can't be a negative value or zero. (Parameter 'top')");
        }

        [Test]
        public static void TopNProductsSold_when_called_with_valid_arguments_should_return_the_correct_result()
        {
            // arrange
            var orders = OrderData();

            // act
            var result = orders.TopNProductsSold(5).ToList();

            // assert
            result.Count.ShouldBeLessThanOrEqualTo(5);
            var first = result.First();
            first.ProductNo.ShouldBe("2");
            first.Quantity.ShouldBe(10);
            var second = result.Skip(1).Take(1).First();
            second.ProductNo.ShouldBe("1");
            second.Quantity.ShouldBe(8);
        }

        private static IEnumerable<Order> OrderData()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "orders.json");
            var json = File.ReadAllText(filePath);

            return JsonSerializer.Deserialize<IEnumerable<Order>>(json);
        }
    }
}
