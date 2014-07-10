namespace Scyano
{
    using System;
    using Core;
    using Tasks;

    public class Scyano : IScyano
    {
        public const string NoValidMessageConsumer = "No message queue consumer provided. Initialize with a valid message consumer!";
        public const string AlreadyInitialized = "Scyano is already initialized. Do not call Initialize() more than once!";
                
        private readonly IMessageConsumerRetriever messageConsumerRetriever;
        private readonly IMessageQueueController messageQueueController;
        private readonly IScyanoTaskExecutor scyanoTaskExecutor;
        private readonly IScyanoFireAndForgetTask scyanoFireAndForgetTask;
        private readonly IDequeueTaskFactory dequeueTaskFactory;

        private object messageConsumer;
        
        public Scyano(
            IMessageConsumerRetriever messageConsumerRetriever,
            IMessageQueueController messageQueueController, 
            IScyanoTaskExecutor scyanoTaskExecutor,
            IScyanoFireAndForgetTask scyanoFireAndForgetTask,
            IDequeueTaskFactory dequeueTaskFactory)
        {
            this.messageConsumerRetriever = messageConsumerRetriever;
            this.messageQueueController = messageQueueController;
            this.scyanoTaskExecutor = scyanoTaskExecutor;
            this.scyanoFireAndForgetTask = scyanoFireAndForgetTask;
            this.dequeueTaskFactory = dequeueTaskFactory;
        }

        public void Initialize(object messageQueueConsumer)
        {
            if (messageQueueConsumer == null)
            {
                throw new ArgumentNullException("messageQueueConsumer", NoValidMessageConsumer);
            }

            if (this.messageConsumer != null)
            {
                throw new InvalidOperationException(AlreadyInitialized);
            }

            this.messageConsumer = messageQueueConsumer;

            var dequeueTask = this.dequeueTaskFactory.Create();
            dequeueTask.Initialize(
               this.messageConsumer,
               this.messageConsumerRetriever.Retrieve(this.messageConsumer),
               this.messageQueueController);
            this.scyanoTaskExecutor.Initialize(dequeueTask);
        }

        public void Start()
        {
            if (this.messageConsumer == null)
            {
                throw new InvalidOperationException("Scyano is not initialized. Initialize Scyano!");
            }
            
            this.scyanoTaskExecutor.Start();
        }

        public void Stop()
        {
            this.scyanoTaskExecutor.Stop();
        }

        public void Enqueue(object message)
        {
            this.scyanoFireAndForgetTask.Run(() => this.messageQueueController.Enqueue(message));
        }
    }
}