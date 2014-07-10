namespace Scyano.NinjectExtensions
{
    using Core;
    using Ninject.Syntax;
    using Tasks;

    public static class BindingRootExtensions
    {
        public static IBindingRoot BindScyano(this IBindingRoot bindingRoot)
        {
            bindingRoot.Bind<IScyano>().To<Scyano>();
            
            bindingRoot.Bind<IScyanoMethodInfo>().To<ScyanoMethodInfo>();
            bindingRoot.Bind<IScyanoTokenSource>().To<ScyanoTokenSource>();
            bindingRoot.Bind<IScyanoTaskExecutor>().To<ScyanoTaskExecutor>();
            bindingRoot.Bind<IScyanoFireAndForgetTask>().To<ScyanoFireAndForgetTask>();
            bindingRoot.Bind<IMessageConsumerRetriever>().To<MessageConsumerRetriever>();
            bindingRoot.Bind<IMessageQueueController>().To<MessageQueueController>();

            bindingRoot.Bind<IDequeueTask>().To<DequeueTask>();
            bindingRoot.Bind<IDequeueTaskFactory>().To<DequeueTaskFactory>();

            return bindingRoot;
        }
    }
}
