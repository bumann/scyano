namespace Scyano.Core
{
    using System;
    using System.Threading;

    public interface IScyanoTokenSource : IDisposable
    {
        CancellationToken Token { get; }

        void Cancel();
    }
}