using System;
using CommonStuff.BE;

namespace ConsoleSearch
{
    public class DocumentHit
    {
        public DocumentHit(BEDocument document, int noOfHits)
        {
            Document = document;
            NoOfHits = noOfHits;
        }

        public BEDocument Document { get;  }

        public int NoOfHits { get;  }
    }
}
