namespace Scyano.Core
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class ScyanoTaskExecutor : IScyanoTaskExecutor
    {
        private readonly object suspender = new object();
        private readonly IScyanoTokenSource scyanoTokenSource;
        private Task worker;

        public ScyanoTaskExecutor(IScyanoTokenSource scyanoTokenSource)
        {
            this.scyanoTokenSource = scyanoTokenSource;
        }

        public bool IsRunning { get; private set; }

        public void Initialize(Action task)
        {
            this.Initialize(new ScyanoTask(task));
        }

        public void Initialize(IScyanoTask task)
        {
            this.worker = new Task(() => this.Run(task), this.scyanoTokenSource.Token);
            this.worker.Start();
        }

        public void StartOrResume()
        {
            if (this.IsRunning)
            {
                return;
            }

            this.IsRunning = true;
            if (Monitor.IsEntered(this.suspender))
            {
                Monitor.Exit(this.suspender);
            }
        }

        public void Suspend()
        {
            if (!this.IsRunning)
            {
                return;
            }

            this.IsRunning = false;
            Monitor.Enter(this.suspender);
        }

        public void Dispose()
        {
            this.scyanoTokenSource.Cancel();
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
                lock (this.suspender)
                {
                }

                task.Execute();
            }
        }
    }
}