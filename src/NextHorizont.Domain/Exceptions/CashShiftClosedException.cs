namespace NextHorizont.Domain.Exceptions;

public class CashShiftClosedException : DomainException
{
    public CashShiftClosedException(string message) : base(message)
    {
    }
}
