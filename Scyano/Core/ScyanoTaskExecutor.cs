namespace Scyano.Core
{
    using System;
    using System.Threading.Tasks;

    public class ScyanoTaskExecutor : IScyanoTaskExecutor
    {
        private readonly IScyanoTokenSourceFactory scyanoTokenSourceFactory;
        private readonly IScyanoFireAndForgetTask scyanoFireAndForgetTask;
        private IScyanoTokenSource scyanoTokenSource;
        private bool isRunning;
        private Task worker;

        public ScyanoTaskExecutor(IScyanoTokenSourceFactory scyanoTokenSourceFactory, IScyanoFireAndForgetTask scyanoFireAndForgetTask)
        {
            this.scyanoTokenSourceFactory = scyanoTokenSourceFactory;
            this.scyanoFireAndForgetTask = scyanoFireAndForgetTask;
        }

        public void Start(IScyanoTask task)
        {
            if (this.isRunning)
            {
                return;
            }

            this.scyanoTokenSource = this.scyanoTokenSourceFactory.Create();
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