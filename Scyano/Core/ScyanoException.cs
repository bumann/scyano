namespace Scyano.Core
{
    using System;

    public class ScyanoException : Exception
    {
        public ScyanoException()
        {
        }

        public ScyanoException(string message)
            : base(message)
        {
        }

        public ScyanoException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}