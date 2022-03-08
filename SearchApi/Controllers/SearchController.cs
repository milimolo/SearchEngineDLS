using ConsoleSearch.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConsoleSearch
{
    [ApiController]
    [Route("search")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;
        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet]
        [Route("{search}")]
        public SearchResult GetSearchResult(string[] query, int maxAmount)
        {
            return _searchService.Search(query, maxAmount);
        }
    }
}
