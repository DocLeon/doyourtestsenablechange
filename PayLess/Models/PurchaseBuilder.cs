using PayLess.Errors;
using PayLess.Validation;

namespace PayLess.Models
{
	public class PurchaseBuilder: IBuildPurchases
	{
		private readonly IValidatePurchaseCanBeBuilt _validate;
		private IParseQueryStrings _queryString;

		public PurchaseBuilder(IValidatePurchaseCanBeBuilt validate, IParseQueryStrings queryString)
		{
			_validate = validate;
			_queryString = queryString;
		}

		public Purchase From(string purchaseParameters)
		{
			var purchaseFields = _queryString.Parse(purchaseParameters);
			_validate.CanBuildPurchaseFrom(purchaseParameters, purchaseFields);

			var paymentType = string.Empty;
			if (purchaseFields.ContainsKey("type"))
				paymentType = purchaseFields["type"];
			CheckAmount(purchaseFields["location"], purchaseFields["amount"], paymentType);
			
			return new Purchase(
				purchaseFields["accountnumber"], 
				purchaseFields["location"], 
				purchaseFields["amount"], 
				purchaseFields["currency"]);
		}

		private void CheckAmount(string location, string amount, string type)
		{
			var purchaseAmount = float.Parse(amount);
			if (location == "GB")
				if ((type == "micro") && (purchaseAmount >= 5)) throw new AmountTooHighForMicroPayment
				{
					Code = "1720000-38",
					Details = string.Format("amount {0} is too high for micropayment in {1}", amount, location)
				};
			if (location == "AU")
				if ((type == "micro") && (purchaseAmount >= 6.09)) throw new AmountTooHighForMicroPayment
				{
					Code = "1720000-38",
					Details = string.Format("amount {0} is too high for micropayment in {1}", amount, location)
				};

		}
	}
}