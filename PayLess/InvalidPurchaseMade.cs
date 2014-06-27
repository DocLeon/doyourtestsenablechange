using System;

namespace PayLess
{
	public class InvalidPurchaseMade : Exception
	{
		public int ErrorCode { get; set; }

		public string Reason { get; set; }
	}
}