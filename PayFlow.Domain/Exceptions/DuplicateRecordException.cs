namespace PayFlow.Domain.Exceptions;

public class DuplicateRecordException : Exception
{
    public DuplicateRecordException(string message) : base(message) { }
}
