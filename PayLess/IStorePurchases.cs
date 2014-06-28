using System;
using System.Configuration;
using System.Data.SqlClient;
using PayLess.Models;

namespace PayLess
{
	public interface IStorePurchases
	{
		void Add(Purchase purchase);
	}

	public class PurchaseStore : IStorePurchases
	{	
		public void Add(Purchase purchase)
		{
			try
			{
				using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["PayLess"].ConnectionString))
				{
					using (var command = new SqlCommand(string.Format("INSERT INTO Purchase (id,accountnumber,location) VALUES ('{0}','{1}','{2}')", purchase.Id, purchase.AccountNumber, purchase.Location), connection))
					{
						connection.Open();
						command.ExecuteNonQuery();
					}
				}	
			}
			catch (Exception)
			{
				//just in case database goes down				
			}
			
		}	
	}
	
}