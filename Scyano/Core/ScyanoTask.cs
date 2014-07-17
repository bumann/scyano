namespace Scyano.Core
{
    using System;

    public class ScyanoTask : IScyanoTask
    {
        private readonly Action task;

        public ScyanoTask(Action task)
        {
            this.task = task;
        }

        public void Execute()
        {
            this.task();
        }
    }
}