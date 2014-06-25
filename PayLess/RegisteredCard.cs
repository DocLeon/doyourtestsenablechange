namespace PayLess
{
	public class RegisteredCard
	{
		public string Token { get; set; }

		public string Type { get; set; }

		public string Number { get; set; }

		public string CardHolderName { get; set; }		
		
		public string ExpiryDate { get; set; }		
	}
}