namespace OnlineShopping.Shared.Domain.Exceptions;

public class ItemNotFoundException : Exception
{
    public ItemNotFoundException() 
        : base() 
    {
    }

    public ItemNotFoundException(string message) 
        : base(message)
    {
    }
}
