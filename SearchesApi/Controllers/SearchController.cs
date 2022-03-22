using CommonStuff.BE;
using ConsoleSearch.Services;
using Microsoft.AspNetCore.Mvc;
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
            string startTime = DateTime.UtcNow.ToString("HH:mm:ss.fff");
            var result = mSearchLogic.Search(query.Split(","), 10);
            string endTime = DateTime.UtcNow.ToString("HH:mm:ss.fff");

            Logger logger = new Logger {
                StartTime = startTime,
                EndTime = endTime,
                Word = query
            };

            var log = mSearchLogic.AddLog(logger);
            Console.WriteLine(log.Word + " was searched on between " + log.StartTime + " and " + log.EndTime);

            var resultStr = JsonSerializer.Serialize(result);
            return Ok(resultStr);
        }
    }
}
