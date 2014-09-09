namespace Scyano
{
    using Core;
    using Tasks;

    /// <summary>
    /// Factory to create Scyano if no dependency injection (like Ninject,...) is used.
    /// </summary>
    public class ScyanoFactory
    {
        public static IScyano<TMessage> Create<TMessage>()
        {
            var messageQueueController = new MessageQueueController<TMessage>();
            var scyanoTaskExecutor = new ScyanoTaskExecutor(new ScyanoTokenSource());
            var scyanoFireAndForgetTask = new ScyanoFireAndForgetTask();
            var dequeueTask = new DequeueTask<TMessage>();
            return new Scyano<TMessage>(
                messageQueueController,
                scyanoTaskExecutor,
                scyanoFireAndForgetTask,
                dequeueTask);
        }
    }
}