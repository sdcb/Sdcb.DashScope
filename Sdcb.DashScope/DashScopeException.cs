using System;

namespace Sdcb.DashScope;

/// <summary>
/// Represents an exception that is thrown by the DashScope API.
/// </summary>
[Serializable]
public class DashScopeException : Exception
{
    /// <inheritdoc/>
    public DashScopeException(string message) : base(message) { }

    /// <inheritdoc/>
    public DashScopeException(string message, Exception innerException) : base(message, innerException) { }
}
