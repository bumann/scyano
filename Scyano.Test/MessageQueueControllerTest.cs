namespace Scyano
{
    using Core;
    using FluentAssertions;
    using Moq;
    using Xunit;

    public class MessageQueueControllerTest
    {
        private readonly MessageQueueController testee;

        public MessageQueueControllerTest()
        {
            this.testee = new MessageQueueController();
        }

        [Fact]
        public void Enqueue_MustCallMessageGetsQueuedOnExtension()
        {
            var message = new object();
            var extension = new Mock<IScyanoCustomExtension>();
            this.testee.Add(extension.Object);

            this.testee.Enqueue(message);

            extension.Verify(x => x.MessageGetsQueued(message), Times.Once());
        }

        [Fact]
        public void Enqueue_WhenExtensionRemoved_MustNotCallMessageGetsQueuedOnExtension()
        {
            var message = new object();
            var extension = new Mock<IScyanoCustomExtension>();
            this.testee.Add(extension.Object);
            this.testee.Remove(extension.Object);

            this.testee.Enqueue(message);

            extension.Verify(x => x.MessageGetsQueued(message), Times.Never());
        }

        [Fact]
        public void Enqueue_WhenSkipMessageExceptionOccured_MustNotMustCallMessageQueued()
        {
            var message = new object();
            var extension = new Mock<IScyanoCustomExtension>();
            this.testee.Add(extension.Object);
            extension.Setup(x => x.MessageGetsQueued(It.IsAny<object>())).Throws<SkipMessageException>();

            this.testee.Enqueue(message);

            extension.Verify(x => x.MessageQueued(message), Times.Never());
        }

        [Fact]
        public void Enqueue_WhenMessageNotSkipped_MustCallMessageQueuedOnExtension()
        {
            var message = new object();
            var extension = new Mock<IScyanoCustomExtension>();
            this.testee.Add(extension.Object);

            this.testee.Enqueue(message);

            extension.Verify(x => x.MessageQueued(message), Times.Once());
        }

        [Fact]
        public void Enqueue_WhenExtensionRemoved_MustNoCallMessageQueued()
        {
            var message = new object();
            var extension = new Mock<IScyanoCustomExtension>();
            this.testee.Add(extension.Object);
            this.testee.Remove(extension.Object);

            this.testee.Enqueue(message);

            extension.Verify(x => x.MessageQueued(message), Times.Never());
        }

        [Fact]
        public void Dequeue_WhenObjectWasQueued_MustReturnQueuedObject()
        {
            var expected = new object();
            this.testee.Enqueue(expected);

            var result = this.testee.Dequeue();

            result.Should().Be(expected);
        }
    }
}