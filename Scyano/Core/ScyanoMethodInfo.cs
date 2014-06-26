namespace Scyano.Core
{
    using System.Reflection;

    /// <summary>
    /// Wrapper class for proper TDD testing.
    /// </summary>
    internal class ScyanoMethodInfo : IScyanoMethodInfo
    {
        private readonly MethodInfo methodInfo;

        public ScyanoMethodInfo(MethodInfo methodInfo)
        {
            this.methodInfo = methodInfo;
        }

        public object Invoke(object method, object[] parameters)
        {
            return this.methodInfo.Invoke(method, parameters);
        }
    }
}