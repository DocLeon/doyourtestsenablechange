namespace PayLess.Models
{
	public interface IBuildPurchases
	{
		Purchase From(string purchaseParameters);
	}
}