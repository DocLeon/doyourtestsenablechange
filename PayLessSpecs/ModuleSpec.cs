using System;
using ExpectedObjects;
using Moq;
using NUnit.Framework;
using Nancy;
using Nancy.Testing;
using PayLess;

namespace PayLessSpecs
{
	[TestFixture]
	public class ModuleSpec
	{
		private IObscureCardNumber _cardNumberObscurer;

		private Browser _browser;
		private BrowserResponse _response;

		[SetUp]
		public void SetUp()
		{
			_cardNumberObscurer = Mock.Of<IObscureCardNumber>(o => o.Obscure("CARD_NUMBER") == "OBSCURED_CARD_NUMBER");

			_browser = new Browser(with => with.Module<PayLessModule>()
			                                   .Dependency(_cardNumberObscurer)
											   .Dependency(new Card(Mock.Of<IStoreCards>())));
			_response = _browser.Post("payless/cardregistration",
											with =>
											{
												with.HttpRequest();
												with.FormValue("cardtype", "CARD-TYPE");
												with.FormValue("cardnumber", "CARD_NUMBER");												
												with.FormValue("cardholdername","CARDHOLDERNAME");
												with.FormValue("startdate","01/14");
												with.FormValue("expirydate", "01/20");
												with.FormValue("CVV","123");
												with.FormValue("issuenumber","1");
											}
											);
		}

		[Test]
		public void Should_return_registered_card()
		{
			var expectedRegisteredCard = new
				                             {
												Type = "CARD-TYPE",
												CardHolderName = "CARDHOLDERNAME",												
												ExpiryDate = "01/20",
				                             }.ToExpectedObject();
			var actualCardRegistered = _response.Body.DeserializeJson<RegisteredCard>();

			Console.WriteLine(_response.Body.AsString());
			expectedRegisteredCard.ShouldMatch(actualCardRegistered);			
		}

		[Test]
		public void Should_obscure_first_twelve_digits_of_card()
		{						
			Mock.Get(_cardNumberObscurer).Verify(o => o.Obscure("CARD_NUMBER"));
		}

		[Test]
		public void Should_return_obscured_card_number()
		{
			Assert.That(_response.Body.DeserializeJson<RegisteredCard>().Number, Is.EqualTo("OBSCURED_CARD_NUMBER"), "ResponseBody:{0}", _response.Body.AsString());		
		}


		public class RegisteredCard
		{
			public string Type { get; set; }

			public string Number { get; set; }

			public string CardHolderName { get; set; }

			public string ExpiryDate { get; set; }
		}
	}

	[TestFixture]
	public class InvalidInputSpec
	{
		private IObscureCardNumber _cardNumberObscurer;
		private Browser _browser;
		private BrowserResponse _response;

		[SetUp]
		public void SetUp()
		{
			_cardNumberObscurer = Mock.Of<IObscureCardNumber>();
			Mock.Get(_cardNumberObscurer).Setup(o => o.Obscure("CARD_NUMBER")).Throws(new InvalidCardDetails("Error Message"));

			_browser = new Browser(with => with.Module<PayLessModule>()
			                                   .Dependency(_cardNumberObscurer)
											   .Dependency(new Card(Mock.Of<IStoreCards>())));
			_response = _browser.Post("payless/cardregistration",
			                          with =>
				                          {
					                          with.HttpRequest();
					                          with.FormValue("cardtype", "CARD-TYPE");
					                          with.FormValue("cardnumber", "CARD_NUMBER");
					                          with.FormValue("cardholdername", "CARDHOLDERNAME");
					                          with.FormValue("startdate", "01/14");
					                          with.FormValue("expirydate", "01/20");
					                          with.FormValue("CVV", "123");
					                          with.FormValue("issuenumber", "1");
				                          }
				);
		}

		[Test]
		public void should_return_invalid_request_status()
		{
			Assert.That(_response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
		}
	}
}
