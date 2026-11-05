using System.Reflection;
using Unit8_SRP_F24.Contracts;

namespace Unit8_SRP_F24;


class Program
{
    static async Task Main(string[] args)
    {
        var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Unit8_SRP_F24.trades.txt");
        if (tradeStream is null)
        {
            Console.Error.WriteLine("Could not find the embedded trades.txt file.");
            return;
        }
        ILogger logger = new ConsoleLogger();

        ITradeDataProvider restfulDataProvider = new RestfulTradeDataProvider("https://unit9trader.azurewebsites.net/api/TradeData", logger);
        ITradeDataProvider streamDataProvider = new StreamTradeDataProvider(tradeStream, logger);
        ITradeDataProvider urlDataProvider = new URLTradeDataProvider("https://raw.githubusercontent.com/tgibbons-css/CIS3285_CurrencyTraderWeb/refs/heads/master/CurrencyTraderTests/trades_5good.txt", logger);    
        
        ITradeValidator tradeValidator = new SimpleTradeValidator(logger);
        ITradeMapper tradeMapper = new SimpleTradeMapper();
        ITradeParser tradeParser = new SimpleTradeParser(tradeValidator, tradeMapper);
        ITradeStorage tradeStorage = new AdoNetTradeStorage(logger);

        var tradeProcessor = new TradeProcessor(restfulDataProvider, tradeParser, tradeStorage);
        await tradeProcessor.ProcessTradesAsync();

    }
}
