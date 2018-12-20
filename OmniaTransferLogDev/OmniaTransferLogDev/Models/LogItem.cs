using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmniaTransferLogDev.Models
{
    public class LogItem
    {
        public Guid RunId { get; set; }
        public string DataFlowName { get; set; }
        public string Type { get; set; }
        public string Subject { get; set; }
        public string System { get; set; }
        public string Source { get; set; }
        public string Sink { get; set; }
        public int? RowCountSource { get; set; }
        public int? RowCountSink { get; set; }
        public string Status { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? CompletionTime { get; set; }
        public string ErrorMessage { get; set; }
        public string MailRecipient { get; set; }
        public bool FullUpload { get; set; }

    }

}
