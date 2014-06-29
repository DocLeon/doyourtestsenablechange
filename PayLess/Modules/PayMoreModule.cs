using System;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;
using Nancy.Responses.Negotiation;
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

                
                if (IsUsingQueryStringParameters)
                    return Negotiate
                        .WithModel("Querystring Parameters Not Permitted use the Request Body")
                        .WithStatusCode(HttpStatusCode.BadRequest);

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

            Get["/paymore/purchase/{purchaseId}"] = parameters =>
            {
                var purchaseId = GetPurchaseIdFrom(parameters);
                Purchase purchase = purchaseRepository.GetById(purchaseId);                                    
                return Negotiate
                    .WithMediaRangeResponse(MediaRange.FromString("text/html"), new XmlResponse<Purchase>(purchase,"text/xml",new DefaultXmlSerializer()))
                    .WithMediaRangeResponse(MediaRange.FromString("text/xml"), new XmlResponse<Purchase>(purchase,"text/xml",new DefaultXmlSerializer()))
                    .WithMediaRangeResponse(MediaRange.FromString("application/json"), new JsonResponse(purchase,new DefaultJsonSerializer()))
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithContentType("application/json")
                    .WithModel(purchase);
            };
        }

        private bool IsUsingQueryStringParameters
        {
            get
            {
                var numberOfItemsOnTheQueryString = Request.Query.Keys.Count;
                return numberOfItemsOnTheQueryString > 0;
            }
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

            PurchaseBuilder.CheckAmount(purchase.Location, purchase.Amount, null);

            purchase.Validate();
        }
    }
}