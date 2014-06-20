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
        private readonly object locker;
        private readonly Queue<object> messageQueue;
        private readonly CancellationTokenSource cancellationTokenSource;
        private object messageConsumer;
        private MethodInfo messageConsumerMethodInfo;
        private Task worker;

        public Scyano()
        {
            this.locker = new object();
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

            lock (this.locker)
            {
                this.cancellationTokenSource.Cancel();
                Monitor.PulseAll(this.locker);
            }

            var stopTask = new Task(
                () =>
                    {
                        this.worker.Wait(TimeSpan.FromSeconds(4));
                        this.worker = null;
                    });

            stopTask.Start();
        }

        public void Enqueue(object message)
        {
            lock (this.locker)
            {
                this.messageQueue.Enqueue(message);
                Monitor.PulseAll(this.locker);
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
                Monitor.Wait(this.locker);

                object message;
                lock (this.locker)
                {
                    message = this.messageQueue.Dequeue();
                    Monitor.PulseAll(this.locker);
                }

                this.messageConsumerMethodInfo.Invoke(this.messageConsumer, new[] { message });
            }
        }
    }
}