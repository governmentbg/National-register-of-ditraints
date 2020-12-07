using System;
using System.Collections.Generic;

namespace NRZ.Data
{
    public partial class UserLogs
    {
        public string ActionName { get; set; }
        public string Controller { get; set; }
        public string Data { get; set; }
        public long Id { get; set; }
        public string Message { get; set; }
        public string RequestMethod { get; set; }
        public string Result { get; set; }
        public DateTime ServerTimeUtc { get; set; }
        public string UserId { get; set; }
        public string Ip { get; set; }
        public long? Duration { get; set; }
        public long? ResponseStatusCode { get; set; }
    }
}
