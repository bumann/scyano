namespace Scyano.Core
{
    public interface IScyanoTaskExecutor
    {
        void Initialize(IScyanoTask task);

        void StartOrResume();

        void Suspend();
    }
}