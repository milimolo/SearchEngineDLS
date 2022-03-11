using ConsoleSearch.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConsoleSearch
{
    [ApiController]
    [Route("search")]
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
            //string[] array = new string[2] { "the", "peter" };
            return Ok(mSearchLogic.Search(query.Split(","), 10));
        }
    }
}
