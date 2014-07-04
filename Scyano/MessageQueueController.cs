namespace Scyano
{
    using System.Collections.Generic;
    using System.Threading;

    public class MessageQueueController : IMessageQueueController
    {
        private readonly object queueLock;
        private readonly Queue<object> messageQueue;
        private readonly ManualResetEventSlim waitHandle;

        public MessageQueueController()
        {
            this.queueLock = new object();
            this.messageQueue = new Queue<object>();
            this.waitHandle = new ManualResetEventSlim();
        }

        public void Enqueue(object message)
        {
            Monitor.Enter(this.queueLock);

            this.messageQueue.Enqueue(message);

            Monitor.PulseAll(this.queueLock);
            Monitor.Exit(this.queueLock);

            this.waitHandle.Set();
        }

        public object Dequeue()
        {
            Monitor.Enter(this.queueLock);

            if (this.messageQueue.Count == 0)
            {
                Monitor.Exit(this.queueLock);
                this.waitHandle.Wait();
                this.waitHandle.Reset();
                Monitor.Enter(this.queueLock);
            }

            object message = this.messageQueue.Dequeue();
            Monitor.PulseAll(this.queueLock);
            Monitor.Exit(this.queueLock);
            return message;
        }
    }
}