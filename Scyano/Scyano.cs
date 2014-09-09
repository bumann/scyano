namespace Scyano
{
    using System;
    using Core;
    using Tasks;

    public class Scyano<TMessage> : IScyano<TMessage>
    {
        public const string NoValidMessageProcessor = "No message queue consumer provided. Initialize with a valid message consumer!";
        public const string AlreadyInitialized = "Scyano is already initialized. Do not call Initialize() more than once!";
                
        private readonly IMessageQueueController<TMessage> messageQueueController;
        private readonly IScyanoTaskExecutor scyanoTaskExecutor;
        private readonly IScyanoFireAndForgetTask scyanoFireAndForgetTask;
        private readonly IDequeueTask<TMessage> dequeueTask;

        private IMessageProcessor<TMessage> messageProcessor;
        
        public Scyano(
            IMessageQueueController<TMessage> messageQueueController, 
            IScyanoTaskExecutor scyanoTaskExecutor,
            IScyanoFireAndForgetTask scyanoFireAndForgetTask,
            IDequeueTask<TMessage> dequeueTask)
        {
            this.messageQueueController = messageQueueController;
            this.scyanoTaskExecutor = scyanoTaskExecutor;
            this.scyanoFireAndForgetTask = scyanoFireAndForgetTask;
            this.dequeueTask = dequeueTask;
        }

        public int MessageCount 
        {
            get
            {
                return this.messageQueueController.MessageCount;
            }
        }

        public void Initialize(IMessageProcessor<TMessage> processor)
        {
            if (processor == null)
            {
                throw new ArgumentNullException("processor", NoValidMessageProcessor);
            }

            if (this.messageProcessor != null)
            {
                throw new InvalidOperationException(AlreadyInitialized);
            }

            this.messageProcessor = processor;

            this.dequeueTask.Initialize(
               this.messageProcessor,
               this.messageQueueController);
            this.scyanoTaskExecutor.Initialize(this.dequeueTask);
        }

        public void Start()
        {
            if (this.messageProcessor == null)
            {
                throw new InvalidOperationException("Scyano is not initialized. Initialize Scyano!");
            }
            
            this.scyanoTaskExecutor.StartOrResume();
        }

        public void Stop()
        {
            this.scyanoTaskExecutor.Suspend();
        }

        public void Enqueue(TMessage message)
        {
            this.scyanoFireAndForgetTask.Run(() => this.messageQueueController.Enqueue(message));
        }

        public void Add(IScyanoCustomExtension<TMessage> extension)
        {
            this.messageQueueController.Add(extension);
        }

        public void Remove(IScyanoCustomExtension<TMessage> extension)
        {
            this.messageQueueController.Remove(extension);
        }
    }
}