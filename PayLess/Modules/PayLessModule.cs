using System;
using System.Configuration;
using System.Data.SqlClient;
using Nancy;
using PayLess.Models;

namespace PayLess.Modules
{
	public class PayLessModule : NancyModule
	{
		private IBuildPurchases _purchase;

		public PayLessModule(IBuildPurchases purchase)
		{
			_purchase = purchase;
			Post["/makepurchase"] = _  =>
				                        {
											_purchase.From(Request.Url.Query);
											return HttpStatusCode.OK;
										
										};


			Get["/status"] = _ =>
				                 {
					                 
										 using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["PayLess"].ConnectionString))
										 {
											 try
											 {
												 connection.Open();
											 }
											 catch (Exception ex)
											 {
												 return string.Format("Something went wrong: {0}",ex.Message);

											 }
										 }


					                 return "I'm okay, thanks for asking.";
				                 };
		}
	}

	public class PurchaseStore
	{		
		public string Save(Purchase purchase)
		{
			string purchaseToken = Guid.NewGuid().ToString();
			using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["PayLess"].ConnectionString))
			{
				/*using (var command = new SqlCommand(string.Format("INSERT INTO Purchase (cardtoken,purchasetoken,amount) VALUES ('{0}','{1}','{2}'",purchase.CardToken,purchaseToken,purchase.Amount), connection))
				{
					connection.Open();
					command.ExecuteNonQuery();
				}*/
			}				
			return purchaseToken;			
		}
		
	}


	public interface IRegisterCards
	{
		RegisteredCard Register(CardDetails cardDetails);
	}
}