namespace Scyano
{
    using System.Collections.Generic;
    using System.Threading;

    internal class MessageQueueController : IMessageQueueController
    {
        private readonly object queueLock;
        private readonly Queue<object> messageQueue;

        public MessageQueueController()
        {
            this.queueLock = new object();
            this.messageQueue = new Queue<object>();
        }

        public void Enqueue(object message)
        {
            Monitor.Enter(this.queueLock);

            this.messageQueue.Enqueue(message);

            Monitor.PulseAll(this.queueLock);
            Monitor.Exit(this.queueLock);
        }

        public object Dequeue()
        {
            Monitor.Enter(this.queueLock);

            object message = null;
            if (this.messageQueue.Count > 0)
            {
                message = this.messageQueue.Dequeue();
            }

            Monitor.PulseAll(this.queueLock);
            Monitor.Exit(this.queueLock);

            return message;
        }
    }
}