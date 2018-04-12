using System;

namespace KpiView.Api 
{
    public class CallDuration 
    {
        public long Id {get;set;}
        public DateTime StartTime {get;set;}
        public DateTime EndTime {get;set;}
        public decimal DurationMilliseconds {get;set;}
    }
}