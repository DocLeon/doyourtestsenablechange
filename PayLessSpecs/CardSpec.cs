using System;
using NUnit.Framework;
using PayLess;

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
				Card.Register(cardDetails);
				Assert.Fail("No error raised");
			}
			catch (InvalidCardDetails ex)
			{
				Assert.That(ex.Message, Contains.Substring("Expiry Date not valid"));
			}

		}

	}


}
