namespace Scyano.Core
{
    public class ScyanoTokenSourceFactory : IScyanoTokenSourceFactory
    {
        public IScyanoTokenSource Create()
        {
            return new ScyanoTokenSource();
        }
    }
}