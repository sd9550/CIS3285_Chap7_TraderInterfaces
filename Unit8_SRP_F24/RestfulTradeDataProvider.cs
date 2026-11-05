using System.Net.Http.Json;
using Unit8_SRP_F24.Contracts;

namespace Unit8_SRP_F24;

public class RestfulTradeDataProvider : ITradeDataProvider
{
    private static readonly HttpClient SharedHttpClient = new();
    private readonly string url;
    private readonly ILogger logger;
    private readonly HttpClient httpClient;

    public RestfulTradeDataProvider(string url, ILogger logger, HttpClient? httpClient = null)
    {
        this.url = url;
        this.logger = logger;
        this.httpClient = httpClient ?? SharedHttpClient;
    }

    public async Task<IEnumerable<string>> ReadTradeDataAsync()
    {
        logger.LogMessage("Reading trade data from Restful API: " + url);
        var tradeData = await httpClient.GetFromJsonAsync<List<string>>(url);
        return tradeData ?? throw new InvalidOperationException("The trade data API returned an empty response.");
    }
}
