using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PayLess;
using PayLess.Errors;
using PayLess.Models;

namespace PayLessSpecs
{
	[TestFixture]
	public class CardNumberObscurerSpec
	{
		[Test]
		public void should_blank_first_12_digits()
		{
			var cardObscurer = new CardNumberObscurer();
			var obscuredNumber = cardObscurer.Obscure("1234567890123456");
			Assert.That(obscuredNumber, Is.EqualTo("************3456"));
		}

		[TestCase("")]
		[TestCase("12345678901234567880")]
		[TestCase(null)]
		public void should_error_if_card_number_not_valid(string cardNumber)
		{
			var cardObscurer = new CardNumberObscurer();			
			try
			{
				cardObscurer.Obscure(cardNumber);
				Assert.Fail("No error raised");
			}
			catch (InvalidCardDetails ex)
			{
				Assert.That(ex.Message, Contains.Substring("Card Number not valid"));
			}
		}
	}
}
