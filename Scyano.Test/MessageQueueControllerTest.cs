namespace Scyano
{
    using FluentAssertions;
    using Xunit;

    public class MessageQueueControllerTest
    {
        private readonly MessageQueueController testee;

        public MessageQueueControllerTest()
        {
            this.testee = new MessageQueueController();
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