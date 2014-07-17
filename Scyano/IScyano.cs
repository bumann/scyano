namespace Scyano
{
    using Core;

    public interface IScyano
    {
        int MessageCount { get; }

        void Initialize(object messageConsumer);

        void Start();

        void Stop();

        void Enqueue(object message);

        void Add(IScyanoCustomExtension extension);

        void Remove(IScyanoCustomExtension extension);
    }
}
