using System.Collections.Generic;
using System.ComponentModel;
using Mozu.Api;

namespace MozuImport.Processes
{
    public interface IProcessContext
    {
        IApiContext ApiContext { get; set; }
        BackgroundWorker Worker { get; set; }
        IDictionary<string, string> Parameters { get; set; }
        IWorkerProcess Process { get; set; }
    }
}
