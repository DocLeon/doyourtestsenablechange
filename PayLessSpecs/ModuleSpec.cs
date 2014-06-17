using System;
using ExpectedObjects;
using Moq;
using NUnit.Framework;
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
			                                   .Dependency(_cardNumberObscurer));
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
												StartDate = "01/14",
												ExpiryDate = "01/20",
												CVV = "123",
												IssueNumber = "1"
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

			public string StartDate { get; set; }

			public string ExpiryDate { get; set; }

			public string CVV { get; set; }

			public string IssueNumber { get; set; }
		}
	}

	
}
