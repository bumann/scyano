namespace Scyano.Core
{
    public interface IScyanoCustomExtension
    {
        void MessageGetsQueued(object message);

        void MessageQueued(object message);
    }
}