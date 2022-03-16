using System;
using CommonStuff.BE;

namespace CommonStuff.BE
{
    public class DocumentHit
    {
        public DocumentHit(BEDocument document, int noOfHits)
        {
            Document = document;
            NoOfHits = noOfHits;
        }

        public BEDocument Document { get; set; }

        public int NoOfHits { get; set; }
    }
}
