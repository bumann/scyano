namespace Scyano
{
    using Core;
    using Tasks;

    /// <summary>
    /// Factory to create Scyano if no dependency injection (like Ninject,...) is used.
    /// </summary>
    public class ScyanoFactory
    {
        public static IScyano Create()
        {
            var messageConsumerRetriever = new MessageConsumerRetriever();
            var messageQueueController = new MessageQueueController();
            var scyanoTaskExecutor = new ScyanoTaskExecutor(new ScyanoTokenSourceFactory(), new ScyanoFireAndForgetTask());
            var scyanoFireAndForgetTask = new ScyanoFireAndForgetTask();
            var dequeueTask = new DequeueTask();
            return new Scyano(messageConsumerRetriever, messageQueueController, scyanoTaskExecutor, scyanoFireAndForgetTask, dequeueTask);
        }
    }
}