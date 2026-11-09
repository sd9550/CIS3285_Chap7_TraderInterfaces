using Unit8_SRP_F24.Contracts;

namespace Unit8_SRP_F24;

public class URLTradeDataProvider : ITradeDataProvider
{
    private static readonly HttpClient sharedClient = new();
    private readonly string url;
    private readonly ILogger logger;
    private readonly HttpClient client;

    public URLTradeDataProvider(string url, ILogger logger)
        : this(url, logger, sharedClient)
    {
    }

    public URLTradeDataProvider(string url, ILogger logger, HttpClient client)
    {
        this.url = url;
        this.logger = logger;
        this.client = client;
    }

    public IEnumerable<string> ReadTradeData()
    {
        return ReadTradeDataAsync().GetAwaiter().GetResult();
    }

    public async Task<IEnumerable<string>> ReadTradeDataAsync(CancellationToken cancellationToken = default)
    {
        logger.LogMessage("Reading trade data from URL: " + url);
        var tradeData = new List<string>();

        using var response = await client.GetAsync(
            url,
            HttpCompletionOption.ResponseHeadersRead,
            cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        using var reader = new StreamReader(stream);
        while (await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false) is { } line)
        {
            tradeData.Add(line);
        }

        return tradeData;
    }
}
