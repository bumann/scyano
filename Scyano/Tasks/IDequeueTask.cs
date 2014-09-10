namespace Scyano.Tasks
{
    using Core;
    using Scyano.MessageProcessing;

    public interface IDequeueTask<TMessage> : IScyanoTask
    {
        void Initialize(IMessageProcessor<TMessage> processor, IMessageQueueController<TMessage> queueController);
    }
}