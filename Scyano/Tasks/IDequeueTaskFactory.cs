namespace Scyano.Tasks
{
    public interface IDequeueTaskFactory
    {
        IDequeueTask Create();
    }
}