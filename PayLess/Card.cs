using System.Text.RegularExpressions;

namespace PayLess
{
	public class Card : IRegisterCards
	{
		private readonly IStoreCards _cardStore;

		public Card(IStoreCards cardStore)
		{
			_cardStore = cardStore;			
		}

		public RegisteredCard Register(CardDetails details)
		{
			Validate(details);
			var token = _cardStore.Save(details);
			return new RegisteredCard
				       {
						   Token = token,
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
}