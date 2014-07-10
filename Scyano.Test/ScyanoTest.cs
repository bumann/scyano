namespace Scyano
{
    using System;
    using Core;
    using FluentAssertions;
    using Moq;
    using Tasks;
    using Xunit;

    public class ScyanoTest
    {
        private readonly Mock<IMessageConsumerRetriever> consumerRetriever;
        private readonly Mock<IMessageQueueController> queueController;
        private readonly Mock<IScyanoTaskExecutor> taskExecutor;
        private readonly Mock<IScyanoFireAndForgetTask> enqueueTask;
        private readonly Mock<IDequeueTaskFactory> dequeueTaskFactory;
        private readonly Scyano testee;

        public ScyanoTest()
        {
            this.consumerRetriever = new Mock<IMessageConsumerRetriever>();
            this.queueController = new Mock<IMessageQueueController>();
            this.enqueueTask = new Mock<IScyanoFireAndForgetTask>();
            this.dequeueTaskFactory = new Mock<IDequeueTaskFactory>();
            this.taskExecutor = new Mock<IScyanoTaskExecutor>();
            this.testee = new Scyano(
                this.consumerRetriever.Object,
                this.queueController.Object,
                this.taskExecutor.Object,
                this.enqueueTask.Object,
                this.dequeueTaskFactory.Object);
        }

        [Fact]
        public void Initialize_WhenInitializedWithoutConsumer_MustThrow()
        {
            this.testee.Invoking(x => x.Initialize(null))
                .ShouldThrow<ArgumentNullException>()
                .WithMessage(Scyano.NoValidMessageConsumer + "\r\nParameter name: messageQueueConsumer");
        }

        [Fact]
        public void Initialize_WhenAlreadyInitialized_MustThrow()
        {
            var consumer = new Consumer();

            this.testee.Initialize(consumer);

            this.testee.Invoking(x => x.Initialize(consumer))
                .ShouldThrow<InvalidOperationException>()
                .WithMessage(Scyano.AlreadyInitialized);
        }

        [Fact]
        public void Initialize_MustRetrieveConsumer()
        {
            var consumer = new Consumer();

            this.testee.Initialize(consumer);

            this.consumerRetriever.Verify(x => x.Retrieve(consumer), Times.Once());
        }

        [Fact]
        public void Start_WhenInitialized_MustExecuteDequeueTask()
        {
            this.testee.Initialize(new Consumer());
            var dequeueTask = Mock.Of<IDequeueTask>();
            this.dequeueTaskFactory.Setup(x => x.Create()).Returns(dequeueTask);

            this.testee.Start();

            this.taskExecutor.Verify(x => x.Start(), Times.Once());
        }

        [Fact]
        public void Start_WhenNotInitialized_MustThrow()
        {
            this.testee.Invoking(x => x.Start())
                .ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void Stop_MustStopTaskExecutor()
        {
            this.testee.Stop();

            this.taskExecutor.Verify(x => x.Stop(), Times.Once());
        }

        [Fact]
        public void Enqueue_MustRunEnqueueTask()
        {
            const string Message = "MyMessage";

            this.testee.Enqueue(Message);

            this.enqueueTask.Verify(x => x.Run(It.IsAny<Action>()));
        }

        [Fact]
        public void Enqueue_MustEnqueue()
        {
            const string Message = "MyMessage";
            this.enqueueTask.Setup(x => x.Run(It.IsAny<Action>()))
                .Callback<Action>(action => action());

            this.testee.Enqueue(Message);

            this.queueController.Verify(x => x.Enqueue(Message), Times.Once());
        }

        private class Consumer
        {
        }
    }
}