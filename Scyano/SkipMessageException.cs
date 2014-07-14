namespace Scyano
{
    using System;
    using Core;

    public class SkipMessageException : ScyanoException
    {
        public SkipMessageException()
        {
        }

        public SkipMessageException(string message)
            : base(message)
        {
        }

        public SkipMessageException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}