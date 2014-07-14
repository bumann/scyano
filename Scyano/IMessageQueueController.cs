namespace Scyano
{
    using System.Collections.Generic;
    using Core;

    public interface IMessageQueueController
    {
        void Initialize(IEnumerable<IScyanoCustomExtension> extensions);

        void Enqueue(object message);

        object Dequeue();
    }
}