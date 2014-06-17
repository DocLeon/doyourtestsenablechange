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
											}
											);
		}

		[Test]
		public void Should_return_registered_card()
		{
			var expectedRegisteredCard = new
				                             {
												Type = "CARD-TYPE"
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
	}

	public class RegisteredCard
	{
		public string Type { get; set; }

		public string Number { get; set; }
	}
}
