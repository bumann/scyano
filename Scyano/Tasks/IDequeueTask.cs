namespace Scyano.Tasks
{
    using Core;

    public interface IDequeueTask : IScyanoTask
    {
        void Initialize(object consumer, IScyanoMethodInfo consumerMethod, IMessageQueueController queueController);
    }
}