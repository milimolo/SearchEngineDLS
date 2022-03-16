using ConsoleSearch.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading;

namespace ConsoleSearch
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        SearchService mSearchLogic;
        public SearchController()
        {
            mSearchLogic = new SearchService(new Database());
        }

        [HttpGet]
        [Route("{query}")]
        public IActionResult GetSearchResult( string query)
        {
            var result = mSearchLogic.Search(query.Split(","), 10);
            var resultStr = JsonSerializer.Serialize(result);
            return Ok(resultStr);
        }
    }
}
