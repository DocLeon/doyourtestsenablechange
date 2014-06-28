using System.Collections.Generic;
using ExpectedObjects;
using NUnit.Framework;
using PayLess;
using PayLess.Models;

namespace PayLessSpecs
{
	[TestFixture]
	public class QueryStringParserSpec
	{
		[Test]
		public void should_return_list_of_fielnames_and_values()
		{
			var queryString = "field1=value1&field2=value2";
			var expected = new Dictionary<string, string>
				               {
					               {"field1","value1"},
								   {"field2", "value2"}
				               }.ToExpectedObject();

			var queryStringPaser = new QueryStringParser();
			var result = queryStringPaser.Parse(queryString);

			expected.ShouldEqual(result);			
		}
	}
}
