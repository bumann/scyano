namespace Scyano
{
    using System;
    using Core;

    public interface IMessageConsumerRetriever
    {
        /// <summary>
        /// Checks the messageQueueConsumer for the MessageConsumer attribute by reflection.
        /// If there is no valid MessageConsumer attribute, an exception is thrown.
        /// </summary>
        /// <param name="messageQueueConsumer">
        /// The messageQueueConsumer to inspect.
        /// </param>
        /// <exception cref="ArgumentException">Thrown if no message consumer method is found.</exception>
        /// <returns>Consumer method.</returns>
        IScyanoMethodInfo Retrieve(object messageQueueConsumer);
    }
}