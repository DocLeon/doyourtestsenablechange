using System;
using NUnit.Framework;
using PayLess;
using PayLess.Errors;
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

		[Test]
		public void should_report_error_if_values_missing()
		{
			var error = Assert.Throws<missingParameterException>(
				()=>new PurchaseStore().PurchaseExists(null,"location","purchaseid"));
			Assert.That(error.Code,Is.EqualTo(17638));
			Assert.That(error.Parameter,Is.EqualTo("accountnumber"));
		}
	}
}
