namespace Scyano
{
    public interface IMessageProcessor<in TMessage>
    {
        void ProcessMessage(TMessage message);
    }
}