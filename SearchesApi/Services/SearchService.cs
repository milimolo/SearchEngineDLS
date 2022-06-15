using System;
using System.Collections.Generic;
using CommonStuff.BE;
using ConsoleSearch.Services;
using SearchAPI.Controllers;

namespace ConsoleSearch
{
    public class SearchService : ISearchService
    {
        Database mDatabase;

        Dictionary<string, int> mWords;

        ResultCache mCache;

        public SearchService(Database database)
        {
            mDatabase = database;
            mWords = mDatabase.GetAllWords();

            mCache = new ResultCache();

        }

        public Logger AddLog(Logger log)
        {
            return mDatabase.AddLog(log);
        }

        /* Perform search of documents containing words from query. The result will
         * contain details about amost maxAmount of documents.
         */
        public SearchResult Search(String[] query, int maxAmount)
        {
            DateTime start = DateTime.Now;

            Array.Sort(query);
            var queryString = String.Join(',', query);
            var result = mCache.GetValue(queryString);
            if (result != null)
            {
                Console.WriteLine("Cache hit: " + queryString);
                result.TimeUsed = DateTime.Now - start;
                return result;
            }
            else
                Console.WriteLine("NO cachehit: " + queryString);

            List<string> ignored;

            // Convert words to wordids
            var wordIds = GetWordIds(query, out ignored);

            // perform the search - get all docIds
            var docIds =  mDatabase.GetDocuments(wordIds);

            // get ids for the first maxAmount             
            var top = new List<int>();
            foreach (var p in docIds.GetRange(0, Math.Min(maxAmount, docIds.Count)))
                top.Add(p.Key);

            // compose the result.
            // all the documentHit
            List<DocumentHit> docresult = new List<DocumentHit>();
            int idx = 0;
            foreach (var doc in mDatabase.GetDocDetails(top))            
                docresult.Add(new DocumentHit(doc, docIds[idx++].Value));


            result = new SearchResult(
                query, 
                docIds.Count,
                docresult, 
                ignored, 
                DateTime.Now - start);

            mCache.Add(queryString, result);

            return result;
        }

        private List<int> GetWordIds(String[] query, out List<string> outIgnored)
        {
            var res = new List<int>();
            var ignored = new List<string>();
            Array.Sort(query);
            foreach (var aWord in query)
            {
                if (mWords.ContainsKey(aWord))
                    res.Add(mWords[aWord]);
                else
                    ignored.Add(aWord);
            }
            outIgnored = ignored;
            return res;
        }
    }
}
