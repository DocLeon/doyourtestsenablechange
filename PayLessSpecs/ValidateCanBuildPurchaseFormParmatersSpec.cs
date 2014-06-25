using Moq;
using NUnit.Framework;
using PayLess;

namespace PayLessSpecs
{
	[TestFixture]
	public class ValidateCanBuildPurchaseFormParmatersSpec
	{
		[Test]
		public void shoud_extract_fields()
		{			
			const string queryString = "QUERYSTRING";
			var fieldParser = Mock.Of<IParseQueryStrings>();
			var validate = new PayLessValidation(fieldParser);
			validate.CanBuildPurchaseFrom(queryString);			
			Mock.Get(fieldParser).Verify(p=>p.Parse(queryString));
		}
	}
}
