using System;

namespace CartridgeBuilder2.Lib.Infrastructure
{
    public class CartridgeBuilderException : Exception
    {
        public CartridgeBuilderException(string message) : base(message)
        {
        }

        public CartridgeBuilderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}