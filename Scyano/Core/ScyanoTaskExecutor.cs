namespace Scyano.Core
{
    using System;
    using System.Threading.Tasks;

    internal class ScyanoTaskExecutor : IScyanoTaskExecutor
    {
        private readonly IScyanoTokenSource scyanoTokenSource;
        private readonly IScyanoFireAndForgetTask scyanoFireAndForgetTask;
        private bool isRunning;
        private Task worker;

        public ScyanoTaskExecutor(IScyanoTokenSource scyanoTokenSource, IScyanoFireAndForgetTask scyanoFireAndForgetTask)
        {
            this.scyanoTokenSource = scyanoTokenSource;
            this.scyanoFireAndForgetTask = scyanoFireAndForgetTask;
        }

        public void Start(IScyanoTask task)
        {
            if (this.isRunning)
            {
                return;
            }

            this.worker = new Task(() => this.Run(task), this.scyanoTokenSource.Token);
            this.worker.Start();
            this.isRunning = true;
        }

        public void Terminate(TimeSpan maxWaitTime)
        {
            if (!this.isRunning)
            {
                return;
            }

            this.scyanoTokenSource.Cancel();
            
            this.scyanoFireAndForgetTask.Run(
                () =>
                {
                    this.worker.Wait(TimeSpan.FromSeconds(4));
                    this.worker = null;
                });

            this.isRunning = false;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.scyanoTokenSource.Dispose();
            }
        }

        private void Run(IScyanoTask task)
        {
            while (!this.worker.IsCanceled)
            {
                task.Execute();
            }
        }
    }
}