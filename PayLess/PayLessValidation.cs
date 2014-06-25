namespace PayLess
{
	public class PayLessValidation: IValidatePurchaseCanBeBuilt
	{
		private IParseQueryStrings _fields;

		public PayLessValidation(IParseQueryStrings fields)
		{
			_fields = fields;
		}

		public void CanBuildPurchaseFrom(string purchaseParams)
		{
			_fields.Parse(purchaseParams);
		}
	}
}