namespace Scyano.Tasks
{
    public class DequeueTask<TMessage> : IDequeueTask<TMessage>
    {
        private IMessageProcessor<TMessage> messageProcessor;
        private IMessageQueueController<TMessage> messageQueueController;

        public void Initialize(IMessageProcessor<TMessage> processor, IMessageQueueController<TMessage> queueController)
        {
            this.messageProcessor = processor;
            this.messageQueueController = queueController;
        }

        public void Execute()
        {
            TMessage message = this.messageQueueController.Dequeue();
            if (message != null)
            {
                this.messageProcessor.ProcessMessage(message);
            }
        }

    }
}