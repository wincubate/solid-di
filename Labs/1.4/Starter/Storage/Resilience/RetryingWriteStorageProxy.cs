using Polly;
using System;
using System.Threading.Tasks;

namespace Wincubate.Solid
{
    class RetryingWriteStorageProxy : IWriteStorage
    {
        private readonly IWriteStorage _proxee;

        public RetryingWriteStorageProxy(IWriteStorage proxee) =>
            _proxee = proxee;

        public async Task StoreDataAsStringAsync(string outputDataAsString)
        {
            try
            {
                IAsyncPolicy policy = Policy
                    .Handle<Exception>()
                    .WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(2))
                    ;
                await policy.ExecuteAsync(() => _proxee.StoreDataAsStringAsync(outputDataAsString));
            }
            catch (Exception exception)
            {
                string message = $"An exception occurred after retrying";
                throw new StockStorageException(message, exception);
            }
        }
    }
}
