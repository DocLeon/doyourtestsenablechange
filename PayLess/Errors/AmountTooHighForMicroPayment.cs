using System;

namespace PayLess.Errors
{
	public class AmountTooHighForMicroPayment : Exception
	{
		public string Code { get; set; }

		public string Details { get; set; }
	}
}