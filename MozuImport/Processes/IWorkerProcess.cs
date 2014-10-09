namespace MozuImport.Processes
{
    public interface IWorkerProcess
    {
        IProcessContext Context { get; set; }
        int MaxCount { get; set; }
        int ProcessedCount { get; set; }

        /// <summary>
        /// Run the process
        /// </summary>
        void Run(IProcessContext context);

        void ReportProgress(object userData);
        void ReportProgress(int processed, object userData);
        int ComputePercentage(int maxRows, int rowsProcessed);
    }
}