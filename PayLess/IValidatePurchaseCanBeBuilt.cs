using System.Collections.Generic;

namespace PayLess
{
	public interface IValidatePurchaseCanBeBuilt
	{
		void CanBuildPurchaseFrom(string purchaseParams, IDictionary<string, string> purchaseFields);
	}
}