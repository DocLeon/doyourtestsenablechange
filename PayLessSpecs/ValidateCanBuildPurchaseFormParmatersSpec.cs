using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using PayLess;
using PayLess.Errors;
using PayLess.Models;

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
			var exception = Assert.Throws<missingParameterException>(() => validate.CanBuildPurchaseFrom("ANYTHING", _fields));
			Assert.That(exception.Code,Is.EqualTo(missingParameter.Length + 3000));
			Assert.That(exception.Parameter,Is.EqualTo(missingParameter));
		}
	}
}
