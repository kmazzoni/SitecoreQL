using System;
using System.Linq;
using System.Linq.Expressions;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq;
using Sitecore.Data;
using SitecoreQL.Query;

namespace SitecoreQL.Repositories
{
    public interface IReadOnlyRepository<T>
    {
        T GetById(Guid id);
        SearchResults<T> GetAll();
        SearchResults<T> GetMany(Expression<Func<T, bool>> predicate, Func<IQueryable<ItemQuery.GraphQLSearchResultItem>, IOrderedQueryable<ItemQuery.GraphQLSearchResultItem>> orderBy = null, int take = 0, int skip = 0);
    }

    public class ItemRepository : IReadOnlyRepository<ItemQuery.GraphQLSearchResultItem>, IDisposable
    {
        private readonly IProviderSearchContext _searchContext;

        public ItemRepository()
        {
            ISearchIndex index = ContentSearchManager.GetIndex($"sitecore_{Sitecore.Context.Database.Name}_index");
            _searchContext = index.CreateSearchContext();
        }

        public ItemQuery.GraphQLSearchResultItem GetById(Guid id)
        {
            var queryable = _searchContext.GetQueryable<ItemQuery.GraphQLSearchResultItem>();

            var queryId = new ID(id);
            queryable = queryable.Where(x => x.ItemId == queryId)
                .Where(x => x.Language == Sitecore.Context.Language.Name)
                .Where(x => x.IsLatestVersion);

            return queryable.GetResults().Select(x => x.Document).FirstOrDefault();
        }

        public SearchResults<ItemQuery.GraphQLSearchResultItem> GetAll()
        {
            return GetMany(null);
        }

        public SearchResults<ItemQuery.GraphQLSearchResultItem> GetMany(Expression<Func<ItemQuery.GraphQLSearchResultItem, bool>> predicate, Func<IQueryable<ItemQuery.GraphQLSearchResultItem>, IOrderedQueryable<ItemQuery.GraphQLSearchResultItem>> orderBy = null, int take = 0, int skip = 0)
        {
            var queryable = _searchContext.GetQueryable<ItemQuery.GraphQLSearchResultItem>();

            if (predicate != null)
            {
                queryable = queryable.Where(predicate);
            }

            var sortedQuery = orderBy?.Invoke(queryable);
            if (sortedQuery != null)
            {
                queryable = sortedQuery;
            }

            queryable = queryable.Skip(skip);

            if (take > 0)
            {
                queryable = queryable.Take(take);
            }
            
            return queryable.GetResults();
        }

        public void Dispose()
        {
            _searchContext?.Dispose();
        }
    }

    
}