using System.Collections.Generic;
using System.Web;

namespace PayLess.Models
{
	public class QueryStringParser: IParseQueryStrings
	{
		public IDictionary<string, string> Parse(string queryString)
		{
			
			var valuesFromQueryString = HttpUtility.ParseQueryString(queryString);

			var values = new Dictionary<string, string>();
			foreach (var value in valuesFromQueryString.Keys)
			{
				var key = value.ToString();
				values.Add(key,valuesFromQueryString[key]);
			}

			return values;
		}
	}
}