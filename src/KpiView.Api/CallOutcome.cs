using System;

namespace KpiView.Api
{
    public class CallOutcome
    {
        public long Id {get;set;}
        public DateTime Timestamp {get;set;}
        public bool IsError {get;set;}
    }
}