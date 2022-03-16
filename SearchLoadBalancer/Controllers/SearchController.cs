using CommonStuff.BE;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SearchLoadBalancer.Controllers
{
    [ApiController]
    [Route("load/[controller]")]
    public class SearchController : ControllerBase
    {
        private List<int> ports = new List<int>()
        {
            5002,
            5004
        };
        private static int currentPort = 0;

        private static object myLock = new object();
        private HttpClient client;

        public SearchController()
        {
            // Round Robin
            lock (myLock)
            {
                if (currentPort == ports.Count - 1)
                {
                    currentPort = 0;
                }
                else
                {
                    currentPort++;
                }
            }
            client = new HttpClient();
            client.BaseAddress = new Uri($"https://localhost:{ports[currentPort]}/");
        }

        [HttpGet]
        [Route("{query}")]
        public IActionResult Get(string query)
        {
            var response = client.GetAsync("search/" + string.Join(",",query));
            var result = response.GetAwaiter().GetResult().Content.ReadAsStringAsync().Result;
            var SearchResult = JsonSerializer.Deserialize<SearchResult>(result);
            var resultStr = JsonSerializer.Serialize(SearchResult);
            Console.WriteLine("Succesfully completed call on URL: " + client.BaseAddress + "search/" + string.Join(",", query));
            return Ok(resultStr);
        }
    }
}
