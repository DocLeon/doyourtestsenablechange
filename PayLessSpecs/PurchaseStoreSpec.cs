using System;
using NUnit.Framework;
using PayLess;
using PayLess.Models;
using PayLess.Modules;
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
	    public void should_get_purchase_by_id()
        {
            var purchaseStore = new PurchaseStore();
            var purchaseId = Guid.NewGuid().ToString();

            purchaseStore.Add(new Purchase
            {
                Id = purchaseId,
                AccountNumber = "4422222222",
                Amount = "5.22",
                Currency = "GBP",
                Location = "GB"
            });

            var purchaseFromDb = purchaseStore.GetById(purchaseId);

            Assert.That(purchaseFromDb, Is.Not.Null);
            Assert.That(purchaseFromDb.Id, Is.EqualTo(purchaseId));
            Assert.That(purchaseFromDb.Location, Is.EqualTo("GB"));
        }

        [Test]
	    public void should_throw_PurchaseNotFound_when_purchase_isnt_in_db()
	    {
            var purchaseStore = new PurchaseStore();
            var purchaseThatIsNotInDb = Guid.NewGuid().ToString();

	        Assert.Throws<PurchaseNotFound>(() => purchaseStore.GetById(purchaseThatIsNotInDb));

	    }
	}
}
