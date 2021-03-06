﻿using System;
using System.Text.RegularExpressions;
using Moq;
using NUnit.Framework;
using Nancy;
using Nancy.Testing;
using PayLess;
using PayLess.Errors;
using PayLess.Models;
using PayLess.Modules;
using PayLess.Repositories;

namespace PayLessSpecs
{
	[TestFixture]
	public class MakePurchase
	{
		[SetUp]
		public void SetUp()
		{
			_purchaseBuilder = Mock.Of<IBuildPurchases>();
			_purchaseStore = Mock.Of<IStorePurchases>();
			_browser = new Browser(with => with.Module<PayLessModule>()
				.Dependency(_purchaseBuilder)
				.Dependency(_purchaseStore)
				.Dependency(Mock.Of<IFindPurchases>()));
		}
		private Browser _browser;
		private IBuildPurchases _purchaseBuilder;
		private IStorePurchases _purchaseStore;

		[Test]
		public void should_build_purchase_from_parameters()
		{			
			_browser.Post("/payless/makepurchase",
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
			var response = _browser.Post("/payless/makepurchase");
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
			var response = _browser.Post("/payless/makepurchase");
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
			var response = _browser.Post("/payless/makepurchase");
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
			Assert.That(response.Body.AsString(), Is.EqualTo("ERROR:928 Reason"));
		}

		[Test]
		public void should_return_purhcase_id()
		{
			Mock.Get(_purchaseBuilder).Setup(p => p.From(It.IsAny<string>())).Returns(new TestPurchase());
			var response = _browser.Post("/payless/makepurchase");
			Assert.That(new Regex(@"\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-F0-9]{12}\b", RegexOptions.IgnoreCase).IsMatch(response.Body.AsString()), "STATUS {0} BODY: {1} ",response.StatusCode,response.Body.AsString());
		}

		[Test]
		public void should_store_purchase()
		{
			Purchase purchase = new TestPurchase();
			Mock.Get(_purchaseBuilder).Setup(p => p.From(It.IsAny<string>())).Returns(purchase);
			_browser.Post("/payless/makepurchase");
			Mock.Get(_purchaseStore).Verify(p=>p.Add(purchase));
		}
	}	

	public class TestPurchase : Purchase
	{
		public TestPurchase():this(null,null,null,null)
		{			
		}
		public TestPurchase(string accountNumber, string location, string amount, string currency)
		{
		}
	}

	[TestFixture]
	public class RefundSpec
	{
		[Test]
		public void should_look_for_purchase_based_on_location_and_account_number_and_purchase_id()
		{
			var purchaseFinder = Mock.Of<IFindPurchases>();
			var browser = new Browser(with => with.Module<PayLessModule>()
			                                      .Dependency(purchaseFinder)
												  .Dependency(Mock.Of<IBuildPurchases>())
												  .Dependency(Mock.Of<IStorePurchases>()));
			browser.Post("/payless/refund", with =>
				                        {
					                        with.Query("location", "LOCATION");
											with.Query("accountnumber","ACCOUNTNUMBER");
											with.Query("purchaseid","PURCHASEID");
				                        }
										   );
			Mock.Get(purchaseFinder).Verify(f=>f.PurchaseExists("ACCOUNTNUMBER","LOCATION","PURCHASEID"));
		}
	}
}
