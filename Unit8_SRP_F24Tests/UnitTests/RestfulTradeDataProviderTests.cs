using System.Net;
using System.Text;
using Unit8_SRP_F24;
using Unit8_SRP_F24.Contracts;

namespace Unit8_SRP_F24Tests.UnitTests;

[TestClass()]
public class RestfulTradeDataProviderTests
{
    [TestMethod()]
    public async Task TestSize3()
    {
        ILogger logger = new ConsoleLogger();
        using var httpClient = new HttpClient(new StubHttpMessageHandler(
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("[\"GBPUSD,10000,2.25\",\"GBPJPY,50000,178.00\",\"USDEUR,2000,0.90\"]", Encoding.UTF8, "application/json")
            }));
        ITradeDataProvider tradeProvider = new RestfulTradeDataProvider(
            "https://unit9trader.azurewebsites.net/api/TradeData", logger, httpClient);

        IEnumerable<string> trades = await tradeProvider.ReadTradeDataAsync();

        CollectionAssert.AreEqual(
            new[] { "GBPUSD,10000,2.25", "GBPJPY,50000,178.00", "USDEUR,2000,0.90" },
            trades.ToArray());
    }

    private sealed class StubHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage response;

        public StubHttpMessageHandler(HttpResponseMessage response)
        {
            this.response = response;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(response);
        }
    }
}
