namespace ChannelEngine.Shared.Client
{
    using System;

    /// <summary>Wraps errors returned by the ChannelEngine API.</summary>
    public class ChannelEngineClientException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelEngineClientException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ChannelEngineClientException(string message) : base(message) { }
    }
}
