namespace Scyano.Core
{
    using System;
    using System.Threading;

    public class ScyanoTokenSource : IScyanoTokenSource
    {
        private readonly CancellationTokenSource cancellationTokenSource;

        public ScyanoTokenSource()
        {
            this.cancellationTokenSource = new CancellationTokenSource();
        }

        public CancellationToken Token
        {
            get { return this.cancellationTokenSource.Token; }
        }

        public void Cancel()
        {
            this.cancellationTokenSource.Cancel();
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
                this.cancellationTokenSource.Dispose();
            }
        }
    }
}