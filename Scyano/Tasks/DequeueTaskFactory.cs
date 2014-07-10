namespace Scyano.Tasks
{
    public class DequeueTaskFactory : IDequeueTaskFactory
    {
        public IDequeueTask Create()
        {
            return new DequeueTask();
        }
    }
}