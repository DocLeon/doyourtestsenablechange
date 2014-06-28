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
		private readonly IStorePurchases _purchaseStore;

		public PayLessModule(IBuildPurchases purchase, IStorePurchases purchaseStore)
		{
			_purchase = purchase;
			_purchaseStore = purchaseStore;
			Post["/makepurchase"] = _  =>
				                        {
											var thePurchase = _purchase.From(Request.Url.Query);
					                        thePurchase.Id = Guid.NewGuid().ToString();
											_purchaseStore.Add(thePurchase);
					                        return "Thankyou for usig payless. Your purchaseId is " + thePurchase.Id;
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
	public interface IRegisterCards
	{
		RegisteredCard Register(CardDetails cardDetails);
	}
}