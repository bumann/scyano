namespace Scyano.Tasks
{
    using Core;

    public interface IDequeueTask<TMessage> : IScyanoTask
    {
        void Initialize(IMessageProcessor<TMessage> processor, IMessageQueueController<TMessage> queueController);
    }
}