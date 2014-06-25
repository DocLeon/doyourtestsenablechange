using Moq;
using NUnit.Framework;
using PayLess;

namespace PayLessSpecs
{
	[TestFixture]
	public class BuildPurchasesSpec
	{
		[Test]
		public void should_validate_parameters()
		{
			var validator = Mock.Of<IValidatePurchaseCanBeBuilt>();
			var purchase = new PurchaseBuilder(validator);
			purchase.From("PURCHASE_PARAMS");			
			Mock.Get(validator).Verify(v=>v.CanBuildPurchaseFrom("PURCHASE_PARAMS"));
		}
	}
}
