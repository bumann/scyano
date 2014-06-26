namespace Scyano.Core
{
    using System;

    public interface IScyanoTaskExecutor
    {
        void Start(IScyanoTask task);

        void Terminate(TimeSpan maxWaitTime);
    }
}