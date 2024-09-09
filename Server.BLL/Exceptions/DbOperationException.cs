namespace Server.BLL.Exceptions;

public class DbOperationException : Exception
{
    public DbOperationException() : base() { }

    public DbOperationException(string message) : base(message) { }

    public DbOperationException(string message, Exception inner) : base(message, inner) { }
}