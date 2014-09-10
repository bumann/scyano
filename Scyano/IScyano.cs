namespace Scyano
{
    using Core;
    using Scyano.MessageProcessing;

    public interface IScyano<TMessage>
    {
        int MessageCount { get; }

        void Initialize(IMessageProcessor<TMessage> processor);

        void Start();

        void Stop();

        void Enqueue(TMessage message);

        void Add(IScyanoCustomExtension<TMessage> extension);

        void Remove(IScyanoCustomExtension<TMessage> extension);
    }
}
