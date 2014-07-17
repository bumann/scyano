namespace Scyano
{
    using System.Collections.Generic;
    using System.Threading;
    using Core;

    public class MessageQueueController : IMessageQueueController
    {
        private readonly object queueLock;
        private readonly Queue<object> messageQueue;
        private readonly ManualResetEventSlim waitHandle;
        private readonly IList<IScyanoCustomExtension> customExtensions;

        public MessageQueueController()
        {
            this.queueLock = new object();
            this.messageQueue = new Queue<object>();
            this.waitHandle = new ManualResetEventSlim();
            this.customExtensions = new List<IScyanoCustomExtension>();
        }

        public int MessageCount
        {
            get { return this.messageQueue.Count; }
        }

        public void Add(IScyanoCustomExtension extension)
        {
            this.customExtensions.Add(extension);
        }

        public void Remove(IScyanoCustomExtension extension)
        {
            this.customExtensions.Remove(extension);
        }

        public void Enqueue(object message)
        {
            Monitor.Enter(this.queueLock);

            try
            {
                foreach (var extension in this.customExtensions)
                {
                    extension.MessageGetsQueued(message);
                }
            }
            catch (SkipMessageException)
            {
                return;
            }

            this.messageQueue.Enqueue(message);

            foreach (var extension in this.customExtensions)
            {
                extension.MessageQueued(message);
            }

            Monitor.PulseAll(this.queueLock);
            Monitor.Exit(this.queueLock);

            this.waitHandle.Set();
        }

        public object Dequeue()
        {
            Monitor.Enter(this.queueLock);

            if (this.messageQueue.Count == 0)
            {
                this.waitHandle.Reset();
                Monitor.Exit(this.queueLock);
                this.waitHandle.Wait();
                Monitor.Enter(this.queueLock);
            }

            object message = this.messageQueue.Dequeue();
            Monitor.PulseAll(this.queueLock);
            Monitor.Exit(this.queueLock);
            return message;
        }
    }
}