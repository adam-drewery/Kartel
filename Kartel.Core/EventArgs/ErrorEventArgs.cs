using System;

namespace Kartel.EventArgs;

public class ErrorEventArgs : System.EventArgs
{
    public ErrorEventArgs(string message, Exception exception)
    {
        Message = message;
        Exception = exception;
    }

    public string Message { get; }

    public Exception Exception { get; }
}