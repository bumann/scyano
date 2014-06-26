namespace Scyano.Core
{
    using System;
    using System.Threading.Tasks;

    public class ScyanoFireAndForgetTask : IScyanoFireAndForgetTask
    {
        public void Run(Action task)
        {
            Task.Run(task);
        }
    }
}