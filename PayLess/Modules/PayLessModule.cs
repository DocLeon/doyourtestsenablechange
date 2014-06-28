using System;
using System.Configuration;
using System.Data.SqlClient;
using Nancy;
using PayLess.Models;
using PayLess.Repositories;

namespace PayLess.Modules
{
	public class PayLessModule : NancyModule
	{
		private IBuildPurchases _purchase;
		private readonly IStorePurchases _purchaseStore;
		private readonly IFindPurchases _purchaseFinder;

		public PayLessModule(IBuildPurchases purchase, IStorePurchases purchaseStore, IFindPurchases purchaseFinder)
		{
			_purchase = purchase;
			_purchaseStore = purchaseStore;
			_purchaseFinder = purchaseFinder;
			Post["/payless/makepurchase"] = _  =>
				                        {
											var thePurchase = _purchase.From(Request.Url.Query);
					                        thePurchase.Id = Guid.NewGuid().ToString();
											_purchaseStore.Add(thePurchase);
					                        return "Thankyou for using payless. Your purchaseId is " + thePurchase.Id;
				                        };

			Post["/payless/refund"] = _ =>
				                  {
					                  return _purchaseFinder.PurchaseExists(Request.Query.AccountNumber,
					                                                        Request.Query.Location,
					                                                        Request.Query.PurchaseId) ? "Refund: SUCCESS" : "Refund: FAILURE";
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