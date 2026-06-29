namespace PayFlow.Domain.Exceptions;

public class PayrollLockedException : Exception
{
    public PayrollLockedException(string message) : base(message) { }
}
