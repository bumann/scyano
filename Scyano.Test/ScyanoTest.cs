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
        private readonly Mock<IMessageQueueController<object>> queueController;
        private readonly Mock<IScyanoTaskExecutor> taskExecutor;
        private readonly Mock<IScyanoFireAndForgetTask> enqueueTask;
        private readonly Mock<IDequeueTask<object>> dequeueTask;
        private readonly Scyano<object> testee;

        public ScyanoTest()
        {
            this.queueController = new Mock<IMessageQueueController<object>>();
            this.enqueueTask = new Mock<IScyanoFireAndForgetTask>();
            this.dequeueTask = new Mock<IDequeueTask<object>>();
            this.taskExecutor = new Mock<IScyanoTaskExecutor>();
            this.testee = new Scyano<object>(
                this.queueController.Object,
                this.taskExecutor.Object,
                this.enqueueTask.Object,
                this.dequeueTask.Object);
        }

        [Fact]
        public void MessageCount_MustReturnMessageQueueCount()
        {
            const int Value = 22;
            this.queueController.Setup(x => x.MessageCount)
                .Returns(Value);

            int result = this.testee.MessageCount;

            result.Should().Be(Value);
        }

        [Fact]
        public void Initialize_WhenInitializedWithoutConsumer_MustThrow()
        {
            this.testee.Invoking(x => x.Initialize(null))
                .ShouldThrow<ArgumentNullException>()
                .WithMessage(Scyano<object>.NoValidMessageProcessor + "\r\nParameter name: processor");
        }

        [Fact]
        public void Initialize_WhenAlreadyInitialized_MustThrow()
        {
            this.testee.Initialize(Mock.Of<IMessageProcessor<object>>());

            this.testee.Invoking(x => x.Initialize(Mock.Of<IMessageProcessor<object>>()))
                .ShouldThrow<InvalidOperationException>()
                .WithMessage(Scyano<object>.AlreadyInitialized);
        }

        [Fact]
        public void Initialize_MustInitializeDequeueTask()
        {
            var messageProcessor = Mock.Of<IMessageProcessor<object>>();

            this.testee.Initialize(messageProcessor);

            this.dequeueTask.Verify(x => x.Initialize(messageProcessor, this.queueController.Object), Times.Once());
        }

        [Fact]
        public void Initialize_MustInitializeTaskExecutor()
        {
            var messageProcessor = Mock.Of<IMessageProcessor<object>>();

            this.testee.Initialize(messageProcessor);

            this.taskExecutor.Verify(x => x.Initialize(this.dequeueTask.Object), Times.Once());
        }

        [Fact]
        public void Start_WhenInitialized_MustExecuteDequeueTask()
        {
            this.testee.Initialize(Mock.Of<IMessageProcessor<object>>());

            this.testee.Start();

            this.taskExecutor.Verify(x => x.StartOrResume(), Times.Once());
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

            this.taskExecutor.Verify(x => x.Suspend(), Times.Once());
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

        [Fact]
        public void Add_MustAddExtension()
        {
            var extension = new Mock<IScyanoCustomExtension<object>>();

            this.testee.Add(extension.Object);

            this.queueController.Verify(x => x.Add(extension.Object), Times.Once());
        }

        [Fact]
        public void Remove_MustRemoveExtension()
        {
            var extension = new Mock<IScyanoCustomExtension<object>>();

            this.testee.Remove(extension.Object);

            this.queueController.Verify(x => x.Remove(extension.Object), Times.Once());
        }
    }
}