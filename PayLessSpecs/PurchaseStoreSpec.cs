using NUnit.Framework;
using PayLess;
using PayLess.Repositories;

namespace PayLessSpecs
{
	[TestFixture]
	public class PurchaseStoreSpec
	{
		[Test]
		public void should_store_purchase()
		{
			var purhcaseStore = new PurchaseStore();
			purhcaseStore.Add(new TestPurchase());
		}

        [Test]
	    public void should_get_purchase_by_id()
	    {
	        
	    }
	}
}
