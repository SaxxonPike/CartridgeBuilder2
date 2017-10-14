using System;

namespace CartridgeBuilder2.Lib.Infrastructure
{
    /// <inheritdoc />
    /// <summary>
    /// Indicates that a class is to be treated as a service for IoC.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceAttribute : Attribute
    {
    }
}