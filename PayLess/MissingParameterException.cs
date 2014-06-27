using System;

namespace PayLess
{
	public class missingParameterException: Exception
	{
		public string Parameter { get; set; }

		public int Code { get; set; }
	}
}