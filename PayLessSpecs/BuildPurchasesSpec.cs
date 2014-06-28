using System.Collections.Generic;
using ExpectedObjects;
using Moq;
using NUnit.Framework;
using PayLess;
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
}
