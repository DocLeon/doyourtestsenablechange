using System;
using Moq;
using NUnit.Framework;
using PayLess;
using PayLess.Errors;
using PayLess.Models;
using PayLess.Repositories;

namespace PayLessSpecs
{
	[TestFixture]
	public class CardSpec
	{
		[TestCase("")]
		[TestCase(null)]
		[TestCase("10/201")]
		[TestCase("12345")]
		public void register_card_errors_when_expiry_date_not_valid(string expiryDate)
		{
			var cardDetails = new CardDetails
			{
				CardNumber = "1234567890123456",
				ExpiryDate = expiryDate
			};
			try
			{
				new Card(Mock.Of<IStoreCards>()).Register(cardDetails);
				Assert.Fail("No error raised");
			}
			catch (InvalidCardDetails ex)
			{
				Assert.That(ex.Message, Contains.Substring("Expiry Date not valid"));
			}

		}

	}

	[TestFixture]
	public class CardRegistrationSpec
	{
		[Test]
		public void should_persist_card_details()
		{
			var cardDetails = new CardDetails
			{
				CardNumber = "1234567890123456",
				ExpiryDate = "10/20"
			};
			var cardStore = Mock.Of<IStoreCards>();
			var card = new Card(cardStore);
			card.Register(cardDetails);
			Mock.Get(cardStore).Verify(c => c.Save(cardDetails));
		}
		
	}

	
}
