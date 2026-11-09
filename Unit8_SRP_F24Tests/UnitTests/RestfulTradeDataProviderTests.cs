using System;
using System.Collections.Generic;
using Unit8_SRP_F24;
using Unit8_SRP_F24.Contracts;

namespace Unit8_SRP_F24Tests.UnitTests;

[TestClass()]
public class RestfulTradeDataProviderTests
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
    public void TestSize3()
    {
        //Arrange
        ILogger logger = new ConsoleLogger();
        string restfulURL = "https://unit9trader.azurewebsites.net/api/TradeData";

        ITradeDataProvider tradeProvider = new RestfulTradeDataProvider(restfulURL, logger);

        // This is an integration test that depends on a remote service.
        try
        {
            IEnumerable<string> trades = tradeProvider.ReadTradeData();
            Assert.IsNotNull(trades, "The provider should return a trade collection.");
            Assert.IsTrue(countStrings(trades) > 0, "The provider should return at least one trade.");
        }
        catch (Exception exception)
        {
            Assert.Inconclusive($"The remote trade service could not be reached: {exception.Message}");
        }
    }
}
