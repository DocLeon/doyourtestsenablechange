using System;
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
	    public void should_get_purchase_by_id_location_and_accountnumber()
        {
	        var purchaseStore = new PurchaseStore();
	        var testPurchase = new TestPurchase
		                           {
			                           AccountNumber = Guid.NewGuid().ToString(),
									   Id = Guid.NewGuid().ToString(),
									   Location = Guid.NewGuid().ToString()
		                           };
	        purchaseStore.Add(testPurchase);
			Assert.That(purchaseStore.PurchaseExists(testPurchase.AccountNumber,
				testPurchase.Location,
				testPurchase.Id));
        }

		[Test]
		public void should_not_get_purchase_that_does_not_exist()
		{
			var purchaseStore = new PurchaseStore();
			var testPurchase = new TestPurchase
			{
				AccountNumber = Guid.NewGuid().ToString(),
				Id = Guid.NewGuid().ToString(),
				Location = Guid.NewGuid().ToString()
			};
			purchaseStore.Add(testPurchase);
			Assert.IsFalse(purchaseStore.PurchaseExists(testPurchase.AccountNumber,
				testPurchase.Location,
				"NO PURCHASE MADE"));
		}
	}
}
