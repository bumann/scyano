namespace Scyano.Core
{
    public interface IScyanoCustomExtension<TMessage>
    {
        IScyano<TMessage> Scyano { get; set; }

        void MessageGetsQueued(TMessage message);

        void MessageQueued(TMessage message);
    }
}