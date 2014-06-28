using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using PayLess.Errors;
using PayLess.Models;
using PayLess.Modules;

namespace PayLess.Repositories
{
	public class PurchaseStore : IStorePurchases, IFindPurchases
	{	
		public void Add(Purchase purchase)
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

        public void Delete(string purchaseId)
        {
            using (
                var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["PayLess"].ConnectionString)
                )
            {
                sqlConnection.Open();

                var purchase =
                    sqlConnection.Query<int>(
                        "DELETE FROM Purchase WHERE Id = @Id",
                        new { Id = purchaseId }).SingleOrDefault();

                sqlConnection.Close();

            }
        }

        public Purchase GetById(string purchaseId)
        {
            using (
                var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["PayLess"].ConnectionString)
                )
            {
                sqlConnection.Open();

                var purchase =
                    sqlConnection.Query<Purchase>(
                        "SELECT Id,AccountNumber, Location FROM Purchase WHERE Id = @Id",
                        new { Id = purchaseId }).SingleOrDefault();

                sqlConnection.Close();
                if (purchase == null)
                    throw new PurchaseNotFound(string.Format("Could not find PurchaseId={0}", purchaseId));
                return purchase;
            }
        }

		public bool PurchaseExists(string accountnumber, string location, string purchaseid)
		{
			ValidateParametersExist(accountnumber, location, purchaseid);
			using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["PayLess"].ConnectionString))
			{
				using (var command = new SqlCommand(string.Format(
					"SELECT Count(*) FROM Purchase WHERE " +
					"Id = '{0}' AND AccountNumber = '{1}' AND Location = '{2}'"
					, purchaseid, accountnumber, location), connection))
				{
					connection.Open();
					if ((int) command.ExecuteScalar() > 0)
						return true;
				}
			}
			return false;
		}

		private static void ValidateParametersExist(string accountnumber, string location, string purchaseid)
		{
			if (accountnumber == null)
				throw new missingParameterException
					      {
						      Code = 17638,
						      Parameter = "accountnumber"
					      };
			if (location == null)
				throw new missingParameterException
					      {
						      Code = 17638,
						      Parameter = "location"
					      };
			if (purchaseid == null)
				throw new missingParameterException
					      {
						      Code = 17638,
						      Parameter = "purchaseid"
					      };
		}
	}
	
	
    public interface IStorePurchases
    {
        void Add(Purchase purchase);
        Purchase GetById(string purchaseId);
        void Delete(string purchaseId);
    }



}