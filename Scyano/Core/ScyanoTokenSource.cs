namespace Scyano.Core
{
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

        public void Dispose()
        {
            this.cancellationTokenSource.Dispose();
        }

        public void Cancel()
        {
            this.cancellationTokenSource.Cancel();
        }
    }
}