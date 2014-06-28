using Moq;
using NUnit.Framework;
using Nancy;
using Nancy.Testing;
using PayLess;
using PayLess.Errors;
using PayLess.Models;
using PayLess.Modules;

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

		[Test]
		public void shoudld_indicate_bad_request_if_any_parameters_missing()
		{
			Mock.Get(_purchaseBuilder).Setup(p => p.From(It.IsAny<string>())).Throws(new missingParameterException()
				                                                                         {
					                                                                         Parameter = "PARAM",
																							 Code = 3001
				                                                                         });
			var response = _browser.Post("/makepurchase");
			Assert.That(response.StatusCode,Is.EqualTo(HttpStatusCode.BadRequest));
			Assert.That(response.Body.AsString(),Is.EqualTo("ERROR:3001 PARAM missing"));
		}

		[Test]
		public void should_indicate_bad_request_if_any_parameter_value_is_missing()
		{
			Mock.Get(_purchaseBuilder).Setup(p => p.From(It.IsAny<string>())).Throws(new MissingValueException()
			{
				Parameter = "PARAM",
				Code = 3001
			});
			var response = _browser.Post("/makepurchase");
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
			Assert.That(response.Body.AsString(), Is.EqualTo("ERROR:3001 PARAM not supplied"));
		}

		[Test]
		public void should_forbid_the_payment_if_invalid()
		{
			Mock.Get(_purchaseBuilder).Setup(p => p.From(It.IsAny<string>())).Throws(new InvalidPurchaseMade
				                                                                         {
																							 ErrorCode = 928,
																							 Reason = "Reason"
				                                                                         });
			var response = _browser.Post("/makepurchase");
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
			Assert.That(response.Body.AsString(), Is.EqualTo("ERROR:928 Reason"));
		}
	}
}
