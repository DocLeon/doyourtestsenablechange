using System;

namespace PayLess.Errors
{
	public class InvalidPurchaseMade : Exception
	{
		public int ErrorCode { get; set; }

		public string Reason { get; set; }
	}
}