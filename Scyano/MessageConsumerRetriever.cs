namespace Scyano
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Core;

    public class MessageConsumerRetriever : IMessageConsumerRetriever
    {
        internal const string NoMessageConsumerMethodSpecified = "Consumer has no message consumer method. Mark the method with [MessageConsumer].";
        internal const string WrongMessageHandlerSignature = "MessageQueueConsumer has wrong message handler signature. Expected signature is void <MethodName>(<MessageType> message). Method: {0}.{1}";

        /// <summary>
        /// Checks the messageQueueConsumer for the MessageConsumer attribute by reflection.
        /// If there is no valid MessageConsumer attribute, an exception is thrown.
        /// </summary>
        /// <param name="messageQueueConsumer">
        /// The messageQueueConsumer to inspect.
        /// </param>
        /// <exception cref="ArgumentException">Thrown if no message consumer method is found.</exception>
        public IScyanoMethodInfo Retrieve(object messageQueueConsumer)
        {
            var methodInfos = new List<MethodInfo>();

            foreach (MethodInfo methodInfo in messageQueueConsumer.GetType()
                .GetRuntimeMethods()
                .Where(methodInfo => methodInfo.GetCustomAttribute<MessageConsumerAttribute>() != null))
            {
                if (methodInfo.GetParameters().Length != 1)
                {
                    throw new ArgumentException(
                        string.Format(
                            WrongMessageHandlerSignature,
                            methodInfo.DeclaringType.FullName,
                            methodInfo.Name));
                }

                methodInfos.Add(methodInfo);
            }

            if (methodInfos.Count == 0)
            {
                throw new ArgumentException(NoMessageConsumerMethodSpecified, "messageQueueConsumer");
            }

            return new ScyanoMethodInfo(methodInfos.First());
        } 
    }
}