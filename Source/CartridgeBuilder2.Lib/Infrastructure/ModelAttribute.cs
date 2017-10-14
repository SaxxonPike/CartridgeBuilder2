using System;

namespace CartridgeBuilder2.Lib.Infrastructure
{
    /// <inheritdoc />
    /// <summary>
    /// Indicates that the class or struct is a model.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class ModelAttribute : Attribute
    {
    }
}