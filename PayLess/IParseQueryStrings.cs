using System.Collections.Generic;

namespace PayLess
{
	public interface IParseQueryStrings
	{
		IDictionary<string, string> Parse(string queryString);
	}
}