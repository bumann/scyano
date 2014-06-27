namespace Scyano.Tasks
{
    using Core;
    using Moq;
    using Xunit;

    public class DequeueTaskTest
    {
        private readonly DequeueTask testee;

        public DequeueTaskTest()
        {
            this.testee = new DequeueTask();
        }

        public interface IConsumer
        {
        }

        [Fact]
        public void Execute_MustDequeueMessageQueue()
        {
            var queue = new Mock<IMessageQueueController>();
            this.testee.Initialize(Mock.Of<IConsumer>(), Mock.Of<IScyanoMethodInfo>(), queue.Object);

            this.testee.Execute();

            queue.Verify(x => x.Dequeue(), Times.Once());
        }

        [Fact]
        public void Execute_WhenNoMessageAvailable_MustNotInvokeConsumer()
        {
            var scyanoMethodInfo = new Mock<IScyanoMethodInfo>();
            this.testee.Initialize(Mock.Of<IConsumer>(), scyanoMethodInfo.Object, Mock.Of<IMessageQueueController>());

            this.testee.Execute();

            scyanoMethodInfo.Verify(x => x.Invoke(It.IsAny<object>(), It.IsAny<object[]>()), Times.Never());
        }

        [Fact]
        public void Execute_WhenMessageAvailable_MustInvokeConsumer()
        {
            var consumer = new Mock<IConsumer>();
            var scyanoMethodInfo = new Mock<IScyanoMethodInfo>();
            var messageQueueController = new Mock<IMessageQueueController>();
            var message = new object();
            messageQueueController.Setup(x => x.Dequeue())
                .Returns(message);
            this.testee.Initialize(consumer.Object, scyanoMethodInfo.Object, messageQueueController.Object);

            this.testee.Execute();

            scyanoMethodInfo.Verify(x => x.Invoke(consumer.Object, new[] { message }), Times.Once());
        }
    }

}