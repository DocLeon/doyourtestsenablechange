using System;

namespace PayLess
{
	public class MissingValueException : Exception
	{
		public string Parameter { get; set; }

		public int Code { get; set; }
	}
}