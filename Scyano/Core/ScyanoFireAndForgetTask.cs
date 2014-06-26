namespace Scyano.Core
{
    using System;
    using System.Threading.Tasks;

    internal class ScyanoFireAndForgetTask : IScyanoFireAndForgetTask
    {
        public void Run(Action task)
        {
            Task.Run(task);
        }
    }
}