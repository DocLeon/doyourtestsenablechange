using System;
using Moq;
using NUnit.Framework;
using Nancy;
using Nancy.Testing;
using PayLess;

namespace PayLessSpecs
{
	[TestFixture]
	public class MakePurchase
	{
		public MakePurchase()
		{
			_purchaseBuilder = Mock.Of<IBuildPurchases>();
			_browser = new Browser(with => with.Module<PayLessModule>()
				.Dependency(_purchaseBuilder));
		}
		private Browser _browser;
		private IBuildPurchases _purchaseBuilder;

		[Test]
		public void should_build_purchase_from_parameters()
		{			
			_browser.Post("/makepurchase",
			              with => with.Query("PURCHASE_PARAMETER", "VALUE")); 
			Mock.Get(_purchaseBuilder).Verify(b=>b.From("?PURCHASE_PARAMETER=VALUE"));
		}
	}

	
}
