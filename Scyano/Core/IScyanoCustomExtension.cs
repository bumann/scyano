namespace Scyano.Core
{
    public interface IScyanoCustomExtension
    {
        IScyano Scyano { get; set; }

        void MessageGetsQueued(object message);

        void MessageQueued(object message);
    }
}