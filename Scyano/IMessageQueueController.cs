namespace Scyano
{
    using Core;

    public interface IMessageQueueController
    {
        void Add(IScyanoCustomExtension extension);

        void Remove(IScyanoCustomExtension extension);

        void Enqueue(object message);

        object Dequeue();
    }
}