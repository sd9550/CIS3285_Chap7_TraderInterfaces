using System;
using System.Threading;
using System.Threading.Tasks;

namespace Unit8_SRP_F24.Contracts;

public interface ITradeDataProvider
{
    public IEnumerable<string> ReadTradeData();

    public Task<IEnumerable<string>> ReadTradeDataAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(ReadTradeData());
    }
}
