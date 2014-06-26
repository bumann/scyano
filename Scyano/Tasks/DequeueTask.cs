namespace Scyano.Tasks
{
    using Core;

    public class DequeueTask : IDequeueTask
    {
        private object messageConsumer;
        private IScyanoMethodInfo messageConsumerMethodInfo;
        private IMessageQueueController messageQueueController;

        public void Initialize(object consumer, IScyanoMethodInfo consumerMethod, IMessageQueueController queueController)
        {
            this.messageConsumer = consumer;
            this.messageConsumerMethodInfo = consumerMethod;
            this.messageQueueController = queueController;
        }

        public void Execute()
        {
            object message = this.messageQueueController.Dequeue();
            if (message != null)
            {
                this.messageConsumerMethodInfo.Invoke(this.messageConsumer, new[] { message });
            }
        }

    }
}