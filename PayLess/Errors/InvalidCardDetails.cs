using System;

namespace PayLess.Errors
{
	public class InvalidCardDetails : Exception
	{
		public InvalidCardDetails(string message) : base(message)
		{			
		}
	}
}