using NUnit.Framework;
using PayLess;
using PayLess.Errors;
using PayLess.Models;

namespace PayLessSpecs
{
	[TestFixture]
	public class PurchaseSpecs
	{
		[TestCase("001234567890", "GB", "15", "GBP", "account number 001234567890 not GB account number")]
		[TestCase("001234567890", "AU", "15", "AUD", "account number 001234567890 not AU account number")]		
		[TestCase("001234567890", "US", "15", "GBP", "purchases cannot be made in US")]
		[TestCase("01","GB","15", "GBP", "acount number 01 not valid should be 12 digits")]
		[TestCase("440123456789","GB","amount", "GBP","amount is not a valid amount")]
		[TestCase("440123456789","GB","15","AUD","currency AUD not valid for GB")]
		[TestCase("012345678919", "AU", "15", "GBP", "currency GBP not valid for AU")]
		[TestCase("012345678919", "AU", "15", "USD", "currency USD is not a valid currency")]
		public void should_throw_if_account_number_not_valid(string accountNumber, string location,string amount,string currency,string reason)
		{
			var exception = Assert.Throws<InvalidPurchaseMade>(()=>
				new Purchase(accountNumber,
							location,
							amount,
							currency));		
			Assert.That(exception.Reason,Is.EqualTo(reason));
		}
	}


}
