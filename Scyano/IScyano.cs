namespace Scyano
{
    using System;

    public interface IScyano : IDisposable
    {
        void Initialize(object messageConsumer);

        void Start();

        void Stop();

        void Enqueue(object message);
    }
}
