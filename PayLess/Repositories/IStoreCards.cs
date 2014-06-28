using PayLess.Models;

namespace PayLess.Repositories
{
	public interface IStoreCards
	{
		string Save(CardDetails cardDetails);
	}
}