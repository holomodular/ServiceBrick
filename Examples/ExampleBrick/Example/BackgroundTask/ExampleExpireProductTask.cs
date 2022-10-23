using Example.Api;
using Microsoft.Extensions.Logging;
using ServiceBrick;
using ServiceQuery;

namespace Example
{
    public static partial class ExampleExpireProductTask
    {
        public static void QueueWork(this ITaskQueue backgroundTaskQueue)
        {
            backgroundTaskQueue.Queue(new Detail());
        }

        public class Detail : ITaskDetail<Detail, Worker>
        {
            public Detail()
            {
            }
        }

        public class Worker : ITaskWork<Detail, Worker>
        {
            private readonly ILogger<Worker> _logger;
            private readonly IProductApiService _productApiService;

            public Worker(
                ILogger<Worker> logger,
                IProductApiService productApiService)
            {
                _logger = logger;
                _productApiService = productApiService;
            }

            public async Task DoWork(Detail detail, CancellationToken cancellationToken)
            {
                var query = QueryBuilder.New()
                    .IsLessThanOrEqual(nameof(ProductDto.ExpirationDate), DateTimeOffset.UtcNow.ToString())
                    .And()
                    .IsEqual(nameof(ProductDto.IsExpired), false.ToString())
                    .Build();

                var respExpired = await _productApiService.QueryAsync(query);
                if (respExpired.Error || respExpired.List == null || respExpired.List.Count == 0)
                    return;

                foreach (var item in respExpired.List)
                {
                    item.IsExpired = true;
                    var respUpdate = await _productApiService.UpdateAsync(item);
                }
            }
        }
    }
}