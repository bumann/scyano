namespace Scyano.MessageProcessing
{
    using Scyano.Core;

    public interface IMessageQueueController<TMessage>
    {
        int MessageCount { get; }

        void Add(IScyanoCustomExtension<TMessage> extension);

        void Remove(IScyanoCustomExtension<TMessage> extension);

        void Enqueue(TMessage message);

        TMessage Dequeue();
    }
}