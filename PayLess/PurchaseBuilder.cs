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
			return new Purchase(
				purchaseFields["accountnumber"], 
				purchaseFields["location"], 
				purchaseFields["amount"], 
				purchaseFields["currency"]);
		}
	}
}