namespace BLL.Validations;

public class MarketException : Exception
{
	public override string Message { get;}
	public MarketException(string message)
	{
		Message = message;
	}
}
