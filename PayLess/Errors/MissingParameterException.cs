using System;

namespace PayLess.Errors
{
	public class missingParameterException: Exception
	{
		public string Parameter { get; set; }

		public int Code { get; set; }
	}
}