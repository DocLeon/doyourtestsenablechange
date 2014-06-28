using System;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;
using PayLess.Errors;
using PayLess.Models;

namespace PayLess.Modules
{
    public class PayMoreModule : NancyModule
    {
        public PayMoreModule(IBuildPurchases purchaseBuilder)
        {
            Post["/paymore/purchase"] = paramters =>
            {
                var purchase = this.Bind<Purchase>(new BindingConfig
                {
                    BodyOnly = true,
                    IgnoreErrors = false
                });

                Validate(purchase);                
                
                
                return new RedirectResponse(string.Format("/paymore/purchase/{0}", Guid.NewGuid()),
                    RedirectResponse.RedirectType.SeeOther);                
            };
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
}