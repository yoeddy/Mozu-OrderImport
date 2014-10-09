using System.Collections.Generic;
using System.ComponentModel;
using Mozu.Api;

namespace MozuImport.Processes
{
    public class ProcessContext : IProcessContext
    {
        public IApiContext ApiContext { get; set; }
        public BackgroundWorker Worker { get; set; }
        public IDictionary<string, string> Parameters { get; set; }
        public IWorkerProcess Process { get; set; }

        public ProcessContext()
        {
            Parameters = new Dictionary<string, string>();
        }
    }
}
