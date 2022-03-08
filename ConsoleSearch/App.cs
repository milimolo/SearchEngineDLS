using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;

namespace ConsoleSearch
{
    public class App
    {
        public App()
        {
        }

        public void Run()
        {
            HttpClient _client = new HttpClient();
            

            Console.WriteLine("Console Search");
            
            while (true)
            {
                Console.WriteLine("enter search terms - q for quit");
                string input = Console.ReadLine();
                if (input.Equals("q")) break;

                var query = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                Console.WriteLine(string.Format("https://localhost:5001/search/{0}", query));
                var response = _client.GetAsync( string.Format("https://localhost:5001/search/{0}", query));

                string result = (response.Result.Content.ReadAsStringAsync().Result);


                SearchResult resultObject = JsonSerializer.Deserialize<SearchResult>(result);

                if (resultObject.Ignored.Count > 0) {
                    Console.WriteLine("Ignored: ");
                    foreach (var aWord in resultObject.Ignored)
                    {
                        Console.WriteLine(aWord + ", ");
                    }
                }

                int idx = 0;
                foreach (var doc in resultObject.DocumentHits) {
                    Console.WriteLine("" + (idx+1) + ": " + doc.Document.mUrl + " -- contains " + doc.NoOfHits + " search terms");
                    Console.WriteLine("Index time: " + doc.Document.mIdxTime + ". Creation time: " + doc.Document.mCreationTime);
                    Console.WriteLine();
                    idx++;
                }
                Console.WriteLine("Documents: " + resultObject.Hits + ". Time: " + resultObject.TimeUsed.TotalMilliseconds);
            }
        }
    }
}
