namespace Scyano.Core
{
    public interface IScyanoMethodInfo
    {
       object Invoke(object method, object[] parameters);
    }
}