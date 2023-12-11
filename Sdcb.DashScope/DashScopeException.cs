using System;

namespace Sdcb.DashScope;

public class DashScopeException : Exception
{
    public DashScopeException(string message) : base(message) { }
    public DashScopeException(string message, Exception innerException) : base(message, innerException) { }
}
