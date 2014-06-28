using System;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;
using PayLess.Errors;
using PayLess.Models;
using PayLess.Repositories;

namespace PayLess.Modules
{
    public class PayMoreModule : NancyModule
    {
        public PayMoreModule(IStorePurchases purchaseRepository)
        {
            Post["/paymore/purchase"] = paramters =>
            {
                var purchase = this.Bind<Purchase>(new BindingConfig
                {
                    BodyOnly = false,
                    IgnoreErrors = false
                });

                purchase.Id = Guid.NewGuid().ToString();

                Validate(purchase);
                purchaseRepository.Add(purchase);
                                
                return new RedirectResponse(string.Format("/paymore/purchase/{0}", purchase.Id));                
            };

            Delete["/paymore/purchase/{purchaseId}"] = parameters =>
            {
                var purchaseId = GetPurchaseIdFrom(parameters);

                purchaseRepository.Delete(purchaseId);
              
                return HttpStatusCode.OK;
            };
        }

        private static string GetPurchaseIdFrom(dynamic parameters)
        {
            var purchaseId = Guid.Empty;
            var isValidPurchaseId = Guid.TryParse(parameters.purchaseId, out purchaseId);
            if (!isValidPurchaseId)
                throw new PurchaseNotFound("PurchaseId is not a valid id");
            return purchaseId.ToString();
        }

        private void Validate(Purchase purchase)
        {
            if (purchase.AccountNumber == null)
                throw new missingParameterException
                {
                    Parameter = "accountNumber"
                };

            if (purchase.Location == null)
                throw new missingParameterException
                {
                    Parameter = "location"
                };

            if (purchase.Amount == null)
                throw new missingParameterException
                {
                    Parameter = "amount"
                };

            if (purchase.Currency == null)
                throw new missingParameterException
                {
                    Parameter = "currency"
                };

            purchase.Validate();
        }
    }

    public class PurchaseNotFound : Exception
    {
        public PurchaseNotFound(string message) : base(message){}
    }
}