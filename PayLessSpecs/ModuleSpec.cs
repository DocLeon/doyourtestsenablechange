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
		public void should_return_ok_if_all_params_supplied()
		{						
			var response = _browser.Post(string.Format("/makepurchase?accountnumber=1234&"),
				with => with.HttpRequest());
			Console.WriteLine(response.Body.AsString());    
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
		}

		[Test]
		public void should_return_bad_request_if_no_query_string_passed()
		{
			var response = _browser.Post("/makepurchase");
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
		}

		[Test]
		public void should_build_purchase_from_parameters()
		{
			const string purchaseParameters = "PURCHASE_PARAMETERS";
			_browser.Post(string.Format("/makepurchase?{0}", purchaseParameters));
			Mock.Get(_purchaseBuilder).Verify(b=>b.From(purchaseParameters));
		}
	}

	
}
