namespace PayLess
{
	public interface IObscureCardNumber
	{
		string Obscure(string cardNumber);
	}

	class ObscureCardNumber : IObscureCardNumber
	{
		public string Obscure(string cardNumber)
		{
			return string.Empty;
		}
	}
}