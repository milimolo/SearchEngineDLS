using System;
using System.Collections.Generic;
using CommonStuff.BE;

namespace SearchAPI.Controllers
{
    public class ResultCache
    {
        Dictionary<String, SearchResult> mCache;
        Dictionary<String, int> mCacheCount;

        public ResultCache(){
            mCache = new Dictionary<string, SearchResult>();
            mCacheCount = new Dictionary<string, int>();
        }

        public SearchResult GetValue(string key)
        {
            if (mCache.ContainsKey(key))
            {
                mCacheCount[key]++;
                return mCache[key];
            }
            return null;
        }

        public void Add(string key, SearchResult value)
        {
            if (mCache.Count < 10)
            {
                mCache.Add(key, value);
                mCacheCount.Add(key, 1);
            }
            else
            {
                int minCount = Int32.MaxValue;
                string minKey = "";
                foreach (KeyValuePair<string, int> p in mCacheCount)
                {
                    if (p.Value < minCount)
                    {
                        minKey = p.Key;
                        minCount = p.Value;
                    }
                }
                mCache.Remove(minKey);
                Add(key, value);
            }
        }
    }
}
