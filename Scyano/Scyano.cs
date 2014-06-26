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
        private readonly IDequeueTask dequeueTask;

        private object messageConsumer;
        
        public Scyano(
            IMessageConsumerRetriever messageConsumerRetriever,
            IMessageQueueController messageQueueController, 
            IScyanoTaskExecutor scyanoTaskExecutor,
            IScyanoFireAndForgetTask scyanoFireAndForgetTask,
            IDequeueTask dequeueTask)
        {
            this.messageConsumerRetriever = messageConsumerRetriever;
            this.messageQueueController = messageQueueController;
            this.scyanoTaskExecutor = scyanoTaskExecutor;
            this.scyanoFireAndForgetTask = scyanoFireAndForgetTask;
            this.dequeueTask = dequeueTask;
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
            this.dequeueTask.Initialize(
                this.messageConsumer,
                this.messageConsumerRetriever.Retrieve(this.messageConsumer),
                this.messageQueueController);
        }

        public void Start()
        {
            if (this.messageConsumer == null)
            {
                throw new InvalidOperationException("Scyano is not initialized. Initialize Scyano!");
            }

            this.scyanoTaskExecutor.Start(this.dequeueTask);
        }

        public void Stop()
        {
            this.scyanoTaskExecutor.Terminate(TimeSpan.FromSeconds(4));
        }

        public void Enqueue(object message)
        {
            this.scyanoFireAndForgetTask.Run(() => this.messageQueueController.Enqueue(message));
        }
    }
}