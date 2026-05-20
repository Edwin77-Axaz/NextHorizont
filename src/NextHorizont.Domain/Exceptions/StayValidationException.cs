namespace NextHorizont.Domain.Exceptions;

public class StayValidationException : DomainException
{
    public StayValidationException(string message) : base(message)
    {
    }
}
