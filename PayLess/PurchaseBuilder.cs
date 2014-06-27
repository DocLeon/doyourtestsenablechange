namespace PayLess
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
			return new Purchase
				       {
					       Location = purchaseFields["location"],
						   AccountNumber = purchaseFields["accountnumber"],
						   Amount = purchaseFields["amount"],
						   Currency = purchaseFields["currency"]
				       };
		}
	}
}