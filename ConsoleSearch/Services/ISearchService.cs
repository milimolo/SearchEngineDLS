using System;
using System.Collections.Generic;

namespace ConsoleSearch.Services
{
    public interface ISearchService
    {
        SearchResult Search(String[] query, int maxAmount);
    }
}
