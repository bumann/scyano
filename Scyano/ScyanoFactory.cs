﻿namespace Scyano
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
            var scyanoTaskExecutor = new ScyanoTaskExecutor(new ScyanoTokenSource());
            var scyanoFireAndForgetTask = new ScyanoFireAndForgetTask();
            var dequeueTaskFactory = new DequeueTaskFactory();
            return new Scyano(
                messageConsumerRetriever,
                messageQueueController,
                scyanoTaskExecutor,
                scyanoFireAndForgetTask,
                dequeueTaskFactory);
        }
    }
}