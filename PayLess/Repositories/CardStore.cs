using System;
using System.Configuration;
using System.Data.SqlClient;
using PayLess.Models;

namespace PayLess.Repositories
{
	public class CardStore : IStoreCards
	{
		private readonly string connectionString = @"Server=localhost;Database=payless;Trusted_Connection=True;";
		private const string INSERT_COMMAND = "INSERT INTO Card VALUES ('{0}')";

		public CardStore()
		{
			connectionString = ConfigurationManager.ConnectionStrings["PayLess"].ConnectionString;
		}
		public string Save(CardDetails cardDetails)
		{
			using (var connection = new SqlConnection(connectionString))
			{
				var cardNumber = Guid.NewGuid().ToString();
				using (var sqlCommand = new SqlCommand(string.Format(INSERT_COMMAND,cardNumber),connection))
				{
					connection.Open();
					sqlCommand.ExecuteNonQuery();
				}
				return cardNumber;
			}
			
		}
		
	}
}