using System;

namespace PayLess
{
	public class InvalidCardDetails : Exception
	{
		public InvalidCardDetails(string message) : base(message)
		{			
		}
	}
}