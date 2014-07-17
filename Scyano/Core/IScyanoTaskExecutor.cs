namespace Scyano.Core
{
    using System;

    public interface IScyanoTaskExecutor
    {
        void Initialize(Action task);

        void Initialize(IScyanoTask task);

        void StartOrResume();

        void Suspend();
    }
}