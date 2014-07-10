namespace Scyano.Core
{
    public interface IScyanoTaskExecutor
    {
        void Initialize(IScyanoTask task);

        void Start();

        void Stop();
    }
}