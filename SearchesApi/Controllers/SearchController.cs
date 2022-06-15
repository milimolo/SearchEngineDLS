using CommonStuff.BE;
using ConsoleSearch.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

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
            var resultStr = SearchAndLog(query);
            return Ok(resultStr);
        }

        private string SearchAndLog(string query)
        {
            string startTime = DateTime.UtcNow.ToString("HH:mm:ss.fff");
            var result = mSearchLogic.Search(query.Split(","), 10);
            string endTime = DateTime.UtcNow.ToString("HH:mm:ss.fff");

            Logger logger = new Logger
            {
                StartTime = startTime,
                EndTime = endTime,
                Word = query
            };

            var log = mSearchLogic.AddLog(logger);
            Console.WriteLine(log.Word + " was searched on between " + log.StartTime + " and " + log.EndTime);

            return JsonConvert.SerializeObject(result, Formatting.Indented);
        }
    }
}
