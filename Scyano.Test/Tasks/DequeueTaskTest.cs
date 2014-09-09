namespace Scyano.Tasks
{
    using Moq;
    using Xunit;

    public class DequeueTaskTest
    {
        private readonly DequeueTask<object> testee;

        public DequeueTaskTest()
        {
            this.testee = new DequeueTask<object>();
        }

        [Fact]
        public void Execute_MustDequeueMessageQueue()
        {
            var queue = new Mock<IMessageQueueController<object>>();
            this.testee.Initialize(Mock.Of<IMessageProcessor<object>>(), queue.Object);

            this.testee.Execute();

            queue.Verify(x => x.Dequeue(), Times.Once());
        }

        [Fact]
        public void Execute_WhenNoMessageAvailable_MustNotProcessMessage()
        {
            var processor = new Mock<IMessageProcessor<object>>();
            this.testee.Initialize(processor.Object, Mock.Of<IMessageQueueController<object>>());

            this.testee.Execute();

            processor.Verify(x => x.ProcessMessage(It.IsAny<object>()), Times.Never());
        }

        [Fact]
        public void Execute_WhenMessageAvailable_MustProcessMessage()
        {
            var processor = new Mock<IMessageProcessor<object>>();
            var messageQueueController = new Mock<IMessageQueueController<object>> { DefaultValue = DefaultValue.Mock };
            this.testee.Initialize(processor.Object, messageQueueController.Object);

            this.testee.Execute();

            processor.Verify(x => x.ProcessMessage(It.IsAny<object>()), Times.Once());
        }
    }
}