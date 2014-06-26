namespace Scyano.Core
{
    using System;

    public interface IScyanoFireAndForgetTask
    {
        void Run(Action task);
    }
}