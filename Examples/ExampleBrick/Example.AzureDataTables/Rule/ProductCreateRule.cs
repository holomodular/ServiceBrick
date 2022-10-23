using Microsoft.Extensions.Logging;
using ServiceBrick;

namespace Example.AzureDataTables
{
    public partial class ProductCreateRule : BusinessRule
    {
        protected readonly ILogger _logger;

        private readonly ITimezoneService _timezoneService;

        public ProductCreateRule(ILoggerFactory loggerFactory, ITimezoneService timezoneService)
        {
            _logger = loggerFactory.CreateLogger<ProductCreateRule>();
            _timezoneService = timezoneService;
            Priority = PRIORITY_LOW;
        }

        public static void RegisterRule(IBusinessRuleRegistry registry)
        {
            registry.RegisterItem(
                typeof(DomainCreateBeforeEvent<Product>),
                typeof(ProductCreateRule));
        }

        public override Task<IResponse> ExecuteRuleAsync(IBusinessRuleContext context)
        {
            var response = new Response();

            try
            {
                if (context.Object is DomainCreateBeforeEvent<Product> ei)
                {
                    var item = ei.DomainObject;
                    item.PartitionKey = Guid.NewGuid().ToString();
                    item.RowKey = string.Empty;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.AddMessage(ResponseMessage.CreateError(LocalizationResource.ERROR_BUSINESS_RULE));
            }

            return Task.FromResult<IResponse>(response);
        }
    }
}