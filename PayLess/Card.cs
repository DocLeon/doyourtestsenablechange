namespace PayLess
{
	public class Card
	{
		public static RegisteredCard Register(CardDetails details)
		{
			return new RegisteredCard
				       {
					       Type = details.CardType,
						   Number = details.CardNumber,
						   CardHolderName = details.CardHolderName,
						   StartDate = details.StartDate,
						   ExpiryDate = details.ExpiryDate,
						   CVV = details.CVV,
						   IssueNumber = details.IssueNumber
				       };
		}
	}
}