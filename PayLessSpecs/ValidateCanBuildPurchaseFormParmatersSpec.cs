using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using PayLess;

namespace PayLessSpecs
{
	[TestFixture]
	public class ValidateCanBuildPurchaseFormParmatersSpec
	{
		private Dictionary<string, string> _fields;

		[SetUp]
		public void initialise_fields()
		{		
			 _fields = new Dictionary<string, string>
				                                     {
					                                     {"accountnumber", "1234"},
					                                     {"location", "UK"},
					                                     {"amount", "1.99"},
					                                     {"currency", "GBP"}
				                                     };
		}

		[Test]
		public void shoud_extract_fields()
		{			
			const string queryString = "QUERYSTRING";
			var fieldParser = Mock.Of<IParseQueryStrings>(
				p => p.Parse(It.IsAny<string>()) == _fields);
			var validate = new PayLessValidation(fieldParser);
			validate.CanBuildPurchaseFrom(queryString);			
			Mock.Get(fieldParser).Verify(p=>p.Parse(queryString));
		}


		[TestCase("accountnumber")]
		[TestCase("location")]
		[TestCase("amount")]
		[TestCase("currency")]
		public void shoud_throw_missing_parameter_exception(string missingParameter)
		{
			_fields.Remove(missingParameter);
			var fieldParser = Mock.Of<IParseQueryStrings>(
				p => p.Parse(It.IsAny<string>()) == _fields);
			var validate = new PayLessValidation(fieldParser);
			Assert.Throws<MissingParameterException>(() => validate.CanBuildPurchaseFrom("ANYTHING"));
		}
	}
}
