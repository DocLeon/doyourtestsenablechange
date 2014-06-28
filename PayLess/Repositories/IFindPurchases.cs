namespace PayLess.Repositories
{
	public interface IFindPurchases
	{
		bool PurchaseExists(string accountnumber, string location, string purchaseid);
	}
}