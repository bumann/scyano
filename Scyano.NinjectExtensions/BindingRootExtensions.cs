namespace Scyano.NinjectExtensions
{
    using Core;
    using Ninject.Syntax;
    using Scyano.MessageProcessing;
    using Tasks;

    public static class BindingRootExtensions
    {
        public static IBindingRoot BindScyano(this IBindingRoot bindingRoot)
        {
            bindingRoot.Bind(typeof(IScyano<>)).To(typeof(Scyano<>));
            
            bindingRoot.Bind<IScyanoMethodInfo>().To<ScyanoMethodInfo>();
            bindingRoot.Bind<IScyanoTokenSource>().To<ScyanoTokenSource>();
            bindingRoot.Bind<IScyanoTaskExecutor>().To<ScyanoTaskExecutor>();
            bindingRoot.Bind<IScyanoFireAndForgetTask>().To<ScyanoFireAndForgetTask>();
            bindingRoot.Bind(typeof(IMessageQueueController<>)).To(typeof(MessageQueueController<>));

            bindingRoot.Bind(typeof(IDequeueTask<>)).To(typeof(DequeueTask<>));

            return bindingRoot;
        }
    }
}
