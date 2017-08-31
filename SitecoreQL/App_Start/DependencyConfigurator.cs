using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using SitecoreQL.Controllers;
using SitecoreQL.Converters;
using SitecoreQL.Query;
using SitecoreQL.Repositories;
using SitecoreQL.Schema;
using SitecoreQL.Types;

namespace SitecoreQL.App_Start
{
    public class DependencyConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            serviceCollection.AddTransient<ItemQuery>();
            serviceCollection.AddTransient<ItemType>();
            serviceCollection.AddTransient<SearchQueryType>();
            serviceCollection.AddTransient<FilterGraphType>();
            serviceCollection.AddTransient<SortGraphType>();
            serviceCollection.AddTransient<SortDirectionGraphType>();
            serviceCollection.AddTransient<FacetsGraphType>();
            serviceCollection.AddTransient<FacetValueGraphType>();
            serviceCollection.AddTransient<IReadOnlyRepository<ItemQuery.GraphQLSearchResultItem>, ItemRepository>();
            serviceCollection.AddTransient<IArgumentToExpressionConverter, ArgumentToExpressionConverter>();
            serviceCollection.AddTransient<ISchema>(d => new SitecoreSchema(type => (GraphType)d.GetService(type)));
            serviceCollection.AddTransient<SitecoreQLController>();
        }
    }
}