namespace Scyano
{
    public interface IMessageQueueController
    {
        void Enqueue(object message);

        object Dequeue();
    }
}