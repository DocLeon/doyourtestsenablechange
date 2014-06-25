namespace PayLess
{
	public class PurchaseBuilder: IBuildPurchases
	{
		private readonly IValidatePurchaseCanBeBuilt _validate;

		public PurchaseBuilder(IValidatePurchaseCanBeBuilt validate)
		{
			_validate = validate;
		}

		public void From(string purchaseParameters)
		{
			_validate.CanBuildPurchaseFrom(purchaseParameters);
		}
	}
}