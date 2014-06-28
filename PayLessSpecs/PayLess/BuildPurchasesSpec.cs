using System.Collections.Generic;
using ExpectedObjects;
using Moq;
using NUnit.Framework;
using PayLess;
using PayLess.Errors;
using PayLess.Models;
using PayLess.Validation;

namespace PayLessSpecs
{
	[TestFixture]
	public class BuildPurchasesSpec
	{
		private IDictionary<string, string> _parsedQueryString = new Dictionary<string, string>
			                                         {
				                                         {"accountnumber","441234567890"},
				                                         {"location","GB"},
				                                         {"amount","12.99"},
				                                         {"currency","GBP"}
			                                         };

		private IParseQueryStrings _parseQueryStrings;

		[SetUp]
		public void SetUp()
		{
			_parseQueryStrings = Mock.Of<IParseQueryStrings>(parser => parser.Parse(It.IsAny<string>()) == _parsedQueryString);
		}

		[Test]
		public void should_validate_parsed_query_string()
		{
			var validator = Mock.Of < IValidatePurchaseCanBeBuilt >();
			var purchase = new PurchaseBuilder(validator,
												_parseQueryStrings);
			purchase.From("PURCHASE_PARAMS");			
			Mock.Get(validator).Verify(v=>v.CanBuildPurchaseFrom("PURCHASE_PARAMS", _parsedQueryString));
		}

		[Test]
		public void should_build_purchase_from_query_string()
		{			
			var purchase = new PurchaseBuilder(Mock.Of<IValidatePurchaseCanBeBuilt>(), _parseQueryStrings);

			var thePurchase = purchase.From("purchaseParameters");

			new
				{
					AccountNumber = _parsedQueryString["accountnumber"],
					Location = _parsedQueryString["location"],
					Amount = _parsedQueryString["amount"],
					Currency = _parsedQueryString["currency"]
				}.ToExpectedObject()
				.ShouldMatch(thePurchase);
		}
	}
	[TestFixture]
	public class build_micro_purchase_spec
	{
		private IDictionary<string, string> _parsedQueryString = new Dictionary<string, string>
			                                         {
				                                         {"accountnumber","441234567890"},				                                         
				                                         {"currency","GBP"},
														 {"type","micro"},
														 {"location","LOCATION"},
														 {"amount","AMOUNT"}
			                                         };

		private IParseQueryStrings _queryStringParser;

		[TestCase("GB", "441234567890", "4.99", "GBP")]
		[TestCase("AU", "123456789019", "6.08", "AUD")]
		public void micro_payment_less_than_cut_off_builds_successfully(string location, string accountNumber, string amount, string currency)
		{

			_parsedQueryString["type"] = "micro";
			_parsedQueryString["location"] = location;
			_parsedQueryString["amount"] = amount;
			_parsedQueryString["accountnumber"] = accountNumber;
			_parsedQueryString["currency"] = currency;

			_queryStringParser = Mock.Of<IParseQueryStrings>(parser => parser.Parse(It.IsAny<string>()) == _parsedQueryString);
			var validator = Mock.Of<IValidatePurchaseCanBeBuilt>();
			var purchase = new PurchaseBuilder(validator,
												_queryStringParser);
			var thePurchase = purchase.From("some input");
			Assert.That(thePurchase, Is.Not.Null);
		}

		[TestCase("GB", "441234567890", "5.00", "GBP")]
		[TestCase("AU", "123456789019", "6.09", "AUD")]
		public void micro_payment_greater_or_equal_to_cutoff_throws_amount_too_high_error(string location, string accountNumber, string amount, string currency)
		{
			_parsedQueryString["type"] = "micro";
			_parsedQueryString["location"] = location;
			_parsedQueryString["amount"] = amount;
			_parsedQueryString["accountnumber"] = accountNumber;
			_parsedQueryString["currency"] = currency;

			_queryStringParser = Mock.Of<IParseQueryStrings>(parser => parser.Parse(It.IsAny<string>()) == _parsedQueryString);

			var validator = Mock.Of<IValidatePurchaseCanBeBuilt>();
			var purchase = new PurchaseBuilder(validator,
												_queryStringParser);
			var error = Assert.Throws<AmountTooHighForMicroPayment>(() => purchase.From("some_input"));

			Assert.That(error.Code, Is.EqualTo("1720000-38"));
			Assert.That(error.Details, Is.EqualTo(string.Format("amount {0} is too high for micropayment in {1}", amount, location)));
		}
	}
}
