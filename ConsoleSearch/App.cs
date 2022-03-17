using CommonStuff.BE;
using System;
using System.Net.Http;
using System.Text.Json;

namespace ConsoleSearch
{
    public class App
    {
        public App()
        {
        }

        public async void Run()
        {
            HttpClient _client = new HttpClient();


            Console.WriteLine("Console Search");

            while (true)
            {
                Console.WriteLine("enter search terms - q for quit and 0 to run 1000 searches");
                string input = Console.ReadLine();
                if (input.Equals("q")) break;
                if (input.Equals("0"))
                {
                    TestSearches(_client);
                }
                else
                {
                    var query = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                    var response = _client.GetAsync("https://localhost:5001/load/search/" + string.Join(",", query));

                    string result = await response.Result.Content.ReadAsStringAsync();
                    var searchResult = JsonSerializer.Deserialize<SearchResult>(result);

                    if (searchResult.Ignored.Count > 0)
                    {
                        Console.WriteLine("Ignored: ");
                        foreach (var aWord in searchResult.Ignored)
                        {
                            Console.WriteLine(aWord + ", ");
                        }
                    }

                    int idx = 0;
                    foreach (var doc in searchResult.DocumentHits)
                    {
                        Console.WriteLine("" + (idx + 1) + ": " + doc.Document.mUrl + " -- contains " + doc.NoOfHits + " search terms");
                        Console.WriteLine("Index time: " + doc.Document.mIdxTime + ". Creation time: " + doc.Document.mCreationTime);
                        Console.WriteLine();
                        idx++;
                    }
                    Console.WriteLine("Documents: " + searchResult.Hits + ". Time: " + searchResult.TimeUsed.TotalMilliseconds);
                }
            }
        }

        private async void TestSearches(HttpClient client)
        {
            string[] words = { "the", "man", "boy", "day", "night", "lady", "a", "i", "do", "what", "to", "let", "wet", "get", "who", "why", "leave", "milk", "food" };
            Random rnd = new Random();

            for (int i = 0; i < 1000; i++)
            {
                int index = rnd.Next(words.Length);
                await client.GetAsync("https://localhost:5001/load/search/" + string.Join(",", words[index]));
            }
        }
    }
}
