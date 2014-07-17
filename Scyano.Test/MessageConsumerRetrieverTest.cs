namespace Scyano
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using FluentAssertions;
    using Xunit;

    public class MessageConsumerRetrieverTest
    {
        private readonly MessageConsumerRetriever testee;

        public MessageConsumerRetrieverTest()
        {
            this.testee = new MessageConsumerRetriever();
        }

        [Fact]
        public void Retrieve_WhenNoMessageHandlerDefinedByAttribute_MustThrow()
        {
            this.testee.Invoking(x => x.Retrieve(new NoConsumerTestClass()))
                .ShouldThrow<ArgumentException>()
                .WithMessage(MessageConsumerRetriever.NoMessageConsumerMethodSpecified + "\r\nParameter name: messageQueueConsumer");
        }

        [Fact]
        public void Retrieve_WhenMessageHandlerDefinedByAttributeHasWrongSignature_MustThrow()
        {
            this.testee.Invoking(x => x.Retrieve(new ConsumerWithWrongSignatureTestClass()))
                .ShouldThrow<ArgumentException>()
                .WithMessage(string.Format(MessageConsumerRetriever.WrongMessageHandlerSignature, "Scyano.MessageConsumerRetrieverTest+ConsumerWithWrongSignatureTestClass", "Consume"));
        }

        [Fact]
        public void Retrieve_WhenMessageHandlerDefinedByAttribute_MustNotThrow()
        {
            this.testee.Invoking(x => x.Retrieve(new ConsumerTestClass()))
                .ShouldNotThrow<ArgumentException>();
        }

        [Fact]
        public void Retrieve_WhenMessageHandlerDefinedByAttribute_MustReturnMethodInfo()
        {
            var result = this.testee.Retrieve(new ConsumerTestClass());

            result.Should().NotBeNull();
        }

        [ExcludeFromCodeCoverage]
        private class NoConsumerTestClass
        {
        }

        [ExcludeFromCodeCoverage]
        private class ConsumerTestClass
        {
            [MessageConsumer]
            private void Consume(object message)
            {
            }
        }

        [ExcludeFromCodeCoverage]
        private class ConsumerWithWrongSignatureTestClass
        {
            [MessageConsumer]
            private void Consume()
            {
            }
        }
    }
}