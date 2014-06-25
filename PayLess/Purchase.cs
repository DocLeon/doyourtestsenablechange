using System;
using System.Data.SqlClient;

namespace PayLess
{
	public class Purchase : IPurchaseMade
	{
		private readonly CardChecker _cardStore;

		public Purchase(CardChecker cardStore)
		{
			_cardStore = cardStore;
		}

		private Purchase()
		{
			PurchaseToken = Guid.NewGuid().ToString();
		}

		public string PurchaseToken { get; set; }
		public string CardToken { get; set; }
		public string Amount { get; set; }

		public IPurchaseMade WithToken(string token, string amount)
		{
			_cardStore.CheckCard(token);
			return new Purchase
				       {
					       CardToken = token,
						   Amount = amount
				       };
		}
	}

	public interface IPurchaseMade
	{
		string PurchaseToken { get; set; }
		string CardToken { get; set; }
		string Amount { get; set; }
	}

	public class CardChecker
	{
		private const string connectionString = @"Server=localhost;Database=payless;Trusted_Connection=True;";
		private const string CHECK_CARD_EXISTS = @"SELECT TOP 1 1 FROM Card WHERE CardNumber = '{0}'";

		public void CheckCard(string token)
		{
			using (var connection = new SqlConnection(connectionString))
			{
				using (var sqlCommand = new SqlCommand(string.Format(CHECK_CARD_EXISTS, token), connection))
				{
					connection.Open();
					var reader = sqlCommand.ExecuteReader();
					if (!reader.Read())
						throw new UnsuccessfulPurchase("Card does not exist");
				}
			}
		}
	}

	public class UnsuccessfulPurchase : Exception
	{
		public UnsuccessfulPurchase(string errorMessage)
			: base(errorMessage)
		{
		}
	}
}