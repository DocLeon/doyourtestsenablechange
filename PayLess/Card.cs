using System;
using System.Text.RegularExpressions;

namespace PayLess
{
	public class Card
	{
		public static RegisteredCard Register(CardDetails details)
		{
			Validate(details);
			return new RegisteredCard
				       {
					       Type = details.CardType,
						   Number = details.CardNumber,
						   CardHolderName = details.CardHolderName,						   
						   ExpiryDate = details.ExpiryDate,
				       };
		}

		private static void Validate(CardDetails details)
		{
			if ((details.ExpiryDate == null) || (!Regex.Match(details.ExpiryDate,@"^(0[1-9]|1[0-2])\/([0-9]{2})$").Success))
				throw new InvalidCardDetails("Expiry Date not valid");
		}
	}
	public class InvalidCardDetails : Exception
	{
		public InvalidCardDetails(string message) : base(message)
		{			
		}
	}
}