namespace Scyano
{
    using System.Collections.Generic;
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
        public void Enqueue_MustCallMessageGetsQueuedOnAllCustomExtensions()
        {
            var message = new object();
            var extension1 = new Mock<IScyanoCustomExtension>();
            var extension2 = new Mock<IScyanoCustomExtension>();
            this.testee.Initialize(new[] { extension1.Object, extension2.Object });

            this.testee.Enqueue(message);

            extension1.Verify(x => x.MessageGetsQueued(message), Times.Once());
            extension2.Verify(x => x.MessageGetsQueued(message), Times.Once());
        }

        [Fact]
        public void Enqueue_WhenSkipMessageExceptionOccured_MustNotMustCallMessageQueued()
        {
            var message = new object();
            var extension1 = new Mock<IScyanoCustomExtension>();
            var extension2 = new Mock<IScyanoCustomExtension>();
            this.testee.Initialize(new[] { extension1.Object, extension2.Object });
            extension2.Setup(x => x.MessageGetsQueued(It.IsAny<object>())).Throws<SkipMessageException>();

            this.testee.Enqueue(message);

            extension1.Verify(x => x.MessageQueued(message), Times.Never());
            extension2.Verify(x => x.MessageQueued(message), Times.Never());
        }

        [Fact]
        public void Enqueue_WhenMessageNotSkipped_MustCallMessageQueuedOnAllCustomExtensions()
        {
            var message = new object();
            var extension1 = new Mock<IScyanoCustomExtension>();
            var extension2 = new Mock<IScyanoCustomExtension>();
            this.testee.Initialize(new[] { extension1.Object, extension2.Object });

            this.testee.Enqueue(message);

            extension1.Verify(x => x.MessageQueued(message), Times.Once());
            extension2.Verify(x => x.MessageQueued(message), Times.Once());
        }

        [Fact]
        public void Dequeue_WhenObjectWasQueued_MustReturnQueuedObject()
        {
            var expected = new object();
            this.testee.Initialize(new List<IScyanoCustomExtension>());
            this.testee.Enqueue(expected);

            var result = this.testee.Dequeue();

            result.Should().Be(expected);
        }
    }
}