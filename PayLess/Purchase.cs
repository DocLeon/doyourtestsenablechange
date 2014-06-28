namespace PayLess
{
	public class Purchase
	{
		public Purchase(string accountNumber, string location, string amount, string currency)
		{
			AccountNumber = accountNumber;
			Location = location;
			Amount = amount;
			Currency = currency;
			float number;
			if (!float.TryParse(Amount,out number)) PurchaseIsInvalid(BecauseAmountIsInvalid());
			if (AccountNumber.Length != 12) PurchaseIsInvalid(BecauseAccountNumberIsWrongLength());
			if (Currency != "AUD" && Currency != "GBP") PurchaseIsInvalid(BecauseCurrencyIsNotAccepted());
			if (Location != "GB" && Location != "AU") PurchaseIsInvalid(BecauseLocationIsInvalid());
			if (Location == "GB")
			{
				if (!AccountNumber.StartsWith("44")) PurchaseIsInvalid(BecauseAccountNumberIsWrong());
				if (Currency != "GBP") PurchaseIsInvalid(BecauseCurrencyIsInvalid());
			}
				
			if (Location == "AU")
			{
				if (!AccountNumber.EndsWith("19")) PurchaseIsInvalid(BecauseAccountNumberIsWrong());
				if (Currency != "AUD") PurchaseIsInvalid(BecauseCurrencyIsInvalid());
			}				
		}

		protected Purchase()
		{			
		}

		private string BecauseCurrencyIsNotAccepted()
		{
			return string.Format("currency {0} is not a valid currency", Currency);
		}

		private string BecauseCurrencyIsInvalid()
		{
			return string.Format("currency {0} not valid for {1}", Currency, Location);
		}

		private string BecauseAmountIsInvalid()
		{
			return string.Format("{0} is not a valid amount", Amount);
		}

		private string BecauseAccountNumberIsWrongLength()
		{
			return string.Format("acount number {0} not valid should be 12 digits",AccountNumber);
		}

		private string BecauseLocationIsInvalid()
		{
			return string.Format("purchases cannot be made in {0}", Location);
		}

		private string BecauseAccountNumberIsWrong()
		{
			return string.Format("account number {0} not {1} account number", AccountNumber, Location);
		}

		private void PurchaseIsInvalid(string reason)
		{
			throw new InvalidPurchaseMade
				      {
					      Reason = reason
				      };
		}

		public string Location { get; set; }

		public string AccountNumber { get; set; }

		public string Amount { get; set; }

		public string Currency { get; set; }

		public string Id { get; set; }
	}
}