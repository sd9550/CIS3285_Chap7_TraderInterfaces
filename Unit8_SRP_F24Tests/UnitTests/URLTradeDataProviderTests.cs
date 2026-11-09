using System;
using System.Net;
using System.Net.Http;
using System.Reflection;

using Unit8_SRP_F24;
using Unit8_SRP_F24.Contracts;

namespace Unit8_SRP_F24Tests.UnitTests;

    [TestClass()]
    public class URLTradeDataProviderTests
    {
        private int countStrings(IEnumerable<string> collectionOfStrings)
        {
            // count the trades
            int count = 0;
            foreach (var trade in collectionOfStrings)
            {
                count++;
            }
            return count;
        }


        [TestMethod()]
        public async Task TestSize1()
        {
            //Arrange
            ILogger logger = new ConsoleLogger();
            string tradeURL = "https://raw.githubusercontent.com/tgibbons-css/CIS3285_CurrencyTraderWeb/refs/heads/master/CurrencyTraderTests/trades_1good.txt";

            ITradeDataProvider tradeProvider = new URLTradeDataProvider(tradeURL, logger);

            //Act
            IEnumerable<string> trades = await tradeProvider.ReadTradeDataAsync();

            //Assert

            Assert.AreEqual(countStrings(trades), 1);
        }

        [TestMethod()]
        public async Task TestSize3()
        {
            const string responseBody = "AAABBB,1000,1.51\nCCCDDD,2000,1.52\nEEEFFF,3000,1.53\n";
            ILogger logger = new ConsoleLogger();
            string tradeURL = "https://example.test/trades.txt";
            using var client = new HttpClient(new StubHttpMessageHandler(responseBody));

            ITradeDataProvider tradeProvider = new URLTradeDataProvider(tradeURL, logger, client);

            IEnumerable<string> trades = await tradeProvider.ReadTradeDataAsync();

            Assert.AreEqual(3, countStrings(trades));
        }
        
        [TestMethod()]
        public async Task TestSize5()
        {
            //Arrange
            ILogger logger = new ConsoleLogger();
            string tradeURL = "https://raw.githubusercontent.com/tgibbons-css/CIS3285_CurrencyTraderWeb/refs/heads/master/CurrencyTraderTests/trades_5good.txt";

            ITradeDataProvider tradeProvider = new URLTradeDataProvider(tradeURL, logger);

            //Act
            IEnumerable<string> trades = await tradeProvider.ReadTradeDataAsync();

            //Assert

            Assert.AreEqual(countStrings(trades), 5);
        }

        private sealed class StubHttpMessageHandler : HttpMessageHandler
        {
            private readonly string responseBody;

            public StubHttpMessageHandler(string responseBody)
            {
                this.responseBody = responseBody;
            }

            protected override Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(responseBody)
                });
            }
        }
    }
