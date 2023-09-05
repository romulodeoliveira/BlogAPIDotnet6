using System;

namespace BlogAPIDotnet6.Exceptions;

public class PostNotFoundException : Exception
{
    public PostNotFoundException()
    {
    }

    public PostNotFoundException(string message)
        : base(message)
    {
    }

    public PostNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}