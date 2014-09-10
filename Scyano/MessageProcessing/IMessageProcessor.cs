namespace Scyano.MessageProcessing
{
    public interface IMessageProcessor<in TMessage>
    {
        void ProcessMessage(TMessage message);
    }
}