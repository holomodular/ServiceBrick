using Microsoft.Extensions.Logging;
using ServiceBrick;

namespace Example.AzureDataTables
{
    public partial class CategoryQueryRule : BusinessRule
    {
        protected readonly ILogger _logger;

        public CategoryQueryRule(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CategoryQueryRule>();
            Priority = PRIORITY_NORMAL;
        }

        public static void RegisterRule(IBusinessRuleRegistry registry)
        {
            registry.RegisterItem(
                typeof(DomainQueryBeforeEvent<Category>),
                typeof(CategoryQueryRule));
        }

        public override Task<IResponse> ExecuteRuleAsync(IBusinessRuleContext context)
        {
            var response = new Response();

            try
            {
                if (context.Object is DomainQueryBeforeEvent<Category> ei)
                {
                    var item = ei.DomainObject;
                    if (ei.Query == null || ei.Query.Filters == null)
                        return Task.FromResult<IResponse>(response);
                    foreach (var filter in ei.Query.Filters)
                    {
                        if (filter.Properties != null &&
                            filter.Properties.Count > 0)
                        {
                            for (int i = 0; i < filter.Properties.Count; i++)
                            {
                                if (string.Compare(filter.Properties[i], "StorageKey", true) == 0)
                                    filter.Properties[i] = "PartitionKey";
                            }
                        }
                    }
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