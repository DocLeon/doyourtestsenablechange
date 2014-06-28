using System.Collections.Generic;

namespace PayLess.Validation
{
	public interface IValidatePurchaseCanBeBuilt
	{
		void CanBuildPurchaseFrom(string purchaseParams, IDictionary<string, string> purchaseFields);
	}
}