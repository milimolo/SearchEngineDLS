using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;

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
            var result = response.Result.Content.ReadAsStringAsync().Result;
            Console.WriteLine("Succesfully completed call on URL: " + client.BaseAddress + "search/" + string.Join(",", query));
            return Ok(result);
        }
    }
}
