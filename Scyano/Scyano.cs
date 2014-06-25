namespace Scyano
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    public class Scyano : IScyano
    {
        private readonly object queueLock;
        private readonly Queue<object> messageQueue;
        private readonly CancellationTokenSource cancellationTokenSource;
        private object messageConsumer;
        private MethodInfo messageConsumerMethodInfo;
        private Task worker;

        public Scyano()
        {
            this.queueLock = new object();
            this.messageQueue = new Queue<object>();
            this.cancellationTokenSource = new CancellationTokenSource();
        }

        public void Initialize(object messageQueueConsumer)
        {
            if (messageQueueConsumer == null)
            {
                throw new ArgumentNullException("messageQueueConsumer", "'messageQueueConsumer' cannot be null. Initialize with a message consumer object!");
            }

            if (this.messageConsumer != null)
            {
                throw new InvalidOperationException("Scyano is already initialized. Do not call Initialize() more than once!");
            }

            this.messageConsumer = messageQueueConsumer;
            this.RetrieveMessageConsumerMethod(this.messageConsumer);
        }

        public void Start()
        {
            if (this.messageConsumer == null)
            {
                throw new InvalidOperationException("Scyano is not initialized. Initialize Scyano!");
            }

            if (this.worker != null && !this.worker.IsCompleted)
            {
                return;
            }

            this.worker = new Task(() => this.Run(), this.cancellationTokenSource.Token);
            this.worker.Start();
        }

        public void Stop()
        {
            if (this.worker == null)
            {
                return;
            }

            this.cancellationTokenSource.Cancel();

            Task.Run(() => this.WaitForWorkerTermination());
        }

        public void Enqueue(object message)
        {
            Task.Run(() => this.EnqueueMessage(message));
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.cancellationTokenSource.Dispose();
            }
        }

        /// <summary>
        /// Checks the messageQueueConsumer for the MessageConsumer attribute by reflection.
        /// If there is no valid MessageConsumer attribute, an exception is thrown.
        /// </summary>
        /// <param name="messageQueueConsumer">
        /// The messageQueueConsumer to inspect.
        /// </param>
        /// <exception cref="ArgumentException">Thrown if no message consumer method is found.</exception>
        private void RetrieveMessageConsumerMethod(object messageQueueConsumer)
        {
            var methodInfos = new List<MethodInfo>();

            foreach (MethodInfo methodInfo in messageQueueConsumer.GetType()
                .GetRuntimeMethods()
                .Where(methodInfo => methodInfo.GetCustomAttribute<MessageConsumerAttribute>() != null))
            {
                if (methodInfo.GetParameters().Length != 1)
                {
                    throw new ArgumentException(
                        string.Format(
                            "MessageQueueConsumer has wrong message handler signature. Expected signature is void <MethodName>(<MessageType> message). Method: {0}.{1}",
                            methodInfo.DeclaringType.FullName,
                            methodInfo.Name));
                }

                methodInfos.Add(methodInfo);
            }

            if (methodInfos.Count == 0)
            {
                throw new ArgumentException("Consumer has no message consumer method. Mark the method with [MessageConsumer].", "messageQueueConsumer");
            }

            this.messageConsumerMethodInfo = methodInfos.First();
        }

        private void Run()
        {
            while (!this.worker.IsCanceled)
            {
                Monitor.Enter(this.queueLock);

                object message = null;
                if (this.messageQueue.Count > 0)
                {
                    message = this.messageQueue.Dequeue();
                }

                Monitor.PulseAll(this.queueLock);
                Monitor.Exit(this.queueLock);

                if (message != null)
                {
                    this.messageConsumerMethodInfo.Invoke(this.messageConsumer, new[] { message });
                }
            }
        }

        private void EnqueueMessage(object message)
        {
            Monitor.Enter(this.queueLock);
            
            this.messageQueue.Enqueue(message);
            
            Monitor.PulseAll(this.queueLock);
            Monitor.Exit(this.queueLock);
        }

        private void WaitForWorkerTermination()
        {
            this.worker.Wait(TimeSpan.FromSeconds(4));
            this.worker = null;
        }
    }
}