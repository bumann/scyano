namespace Scyano
{
    using System;

    /// <summary>
    /// If a method is marked with this attribute,
    /// it is recognized by Scyano as the sink for messages.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class MessageConsumerAttribute : Attribute
    {
    }
}
