namespace Scyano
{
    public interface IScyano
    {
        void Initialize(object messageConsumer);

        void Start();

        void Stop();

        void Enqueue(object message);
    }
}
