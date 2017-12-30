using System;

namespace PuzzleHunt.Web.Models
{
    public class HintSummary
    {
        public int Order { get; private set; }
        public DateTime RequestTime { get; private set; }

        public HintSummary(int order, DateTime requestTime)
        {
            Order = order;
            RequestTime = requestTime;
        }
    }
}