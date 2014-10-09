namespace MozuImport.Processes
{
    public abstract class AbstractWorkerProcess : IWorkerProcess
    {
        public IProcessContext Context { get; set; }
        public int MaxCount { get; set; }
        public int ProcessedCount { get; set; }

        protected AbstractWorkerProcess()
        {
        }

        /// <summary>
        /// Run the process
        /// </summary>
        public abstract void Run(IProcessContext context);

        public void ReportProgress(object userData)
        {
            ReportProgress(ProcessedCount, userData);
        }

        /// <summary>
        /// Returns true if worker cancellation pending
        /// </summary>
        public bool IsCancellationPending
        {
            get { return Context.Worker.CancellationPending; }
        }

        public void ReportProgress(int processed, object userData)
        {
            // Keep track of current progress
            ProcessedCount = processed;

            // Report the worker progress
            Context.Worker.ReportProgress(
                ComputePercentage(MaxCount, ProcessedCount),
                userData);
        }

        public int ComputePercentage(int maxRows, int rowsProcessed)
        {
            if (maxRows > 0)
            {
                return (int)((rowsProcessed / (double)maxRows) * 100);
            }

            return (int)((rowsProcessed / (double)(rowsProcessed + 1000)) * 100);
        }
    }
}
