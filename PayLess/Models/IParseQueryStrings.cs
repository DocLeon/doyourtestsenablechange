using System.Collections.Generic;

namespace PayLess.Models
{
	public interface IParseQueryStrings
	{
		IDictionary<string, string> Parse(string queryString);
	}
}