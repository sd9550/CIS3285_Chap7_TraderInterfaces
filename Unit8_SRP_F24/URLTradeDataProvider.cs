using Unit8_SRP_F24.Contracts;

namespace Unit8_SRP_F24;

public class URLTradeDataProvider : ITradeDataProvider
{
    private static readonly HttpClient SharedHttpClient = new();
    private readonly string url;        // URL to fetch trade data from
    private readonly ILogger logger;    //  Logger to log messages

    public URLTradeDataProvider(string url, ILogger logger)
    {
        this.url = url;
        this.logger = logger;
    }

    public async Task<IEnumerable<string>> ReadTradeDataAsync()
    {
        logger.LogMessage("Reading trade data from URL: " + url);
        List<string> tradeData = new List<string>();
        using (HttpResponseMessage response = await SharedHttpClient.GetAsync(url))
        {
            if (!response.IsSuccessStatusCode)
            {
                logger.LogMessage("Failed to fetch trade data from URL: " + url);
                throw new InvalidOperationException("Failed to fetch trade data from URL: " + url);
            }
            using (Stream stream = await response.Content.ReadAsStreamAsync())
            using (StreamReader reader = new StreamReader(stream))
            {
                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    tradeData.Add(line);
                }
            }
        }
        return tradeData;
    }
}
