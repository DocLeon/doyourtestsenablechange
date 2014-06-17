namespace PayLess
{
	public interface IObscureCardNumber
	{
		string Obscure(string cardNumber);
	}

	public class CardNumberObscurer : IObscureCardNumber
	{
		public string Obscure(string cardNumber)
		{
			if ((cardNumber == null) || (cardNumber.Length != 16))
				throw new InvalidCardDetails("Card Number not valid");
			var cardDigits = cardNumber.ToCharArray();
			for (var i = 0; i < 12; i++)
				cardDigits[i] = '*';
			return new string(cardDigits);
		}
	}
}