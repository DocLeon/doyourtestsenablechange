using System;
using System.Diagnostics.Eventing.Reader;
using PayLess.Errors;
using PayLess.Validation;

namespace PayLess.Models
{
    public class PurchaseBuilder : IBuildPurchases
    {
        private readonly IValidatePurchaseCanBeBuilt _validate;
        private IParseQueryStrings _queryString;

        public PurchaseBuilder(IValidatePurchaseCanBeBuilt validate, IParseQueryStrings queryString)
        {
            _validate = validate;
            _queryString = queryString;
        }

        public Purchase From(string purchaseParameters)
        {
            var purchaseFields = _queryString.Parse(purchaseParameters);
            _validate.CanBuildPurchaseFrom(purchaseParameters, purchaseFields);

            var paymentType = string.Empty;
            if (purchaseFields.ContainsKey("type"))
                paymentType = purchaseFields["type"];
            CheckAmount(purchaseFields["location"], purchaseFields["amount"], paymentType);

            return new Purchase(
                purchaseFields["accountnumber"],
                purchaseFields["location"],
                purchaseFields["amount"],
                purchaseFields["currency"]);
        }

        public static void CheckAmount(string location, string amount, string type)
        {
            var purchaseAmount = decimal.Parse(amount);
            if (location == "GB")
            {
                if ((type == "micro") && (purchaseAmount >= 5m))
                    throw new AmountTooHighForMicroPayment
                    {
                        Code = "1720000-38",
                        Details = string.Format("amount {0} is too high for micropayment in {1}", amount, location)
                    };
                if (type != "micro" && purchaseAmount < 5m)
                    throw new AmountTooLowForPaymore
                    {
                        Code = "1720000-39",
                        Details = string.Format("amount {0} is too low for PayMore in {1}", purchaseAmount, location)
                    };
            }
            if (location == "AU")
            {
                if ((type == "micro") && (purchaseAmount >= 6.09m))
                    throw new AmountTooHighForMicroPayment
                    {
                        Code = "1720000-38",
                        Details = string.Format("amount {0} is too high for micropayment in {1}", amount, location)
                    };
                if (type != "micro" && purchaseAmount < 6.09m)
                    throw new AmountTooLowForPaymore
                    {
                        Code = "1720000-39",
                        Details = string.Format("amount {0} is too low for PayMore in {1}", purchaseAmount, location)
                    };
            }

        }
    }

    public class AmountTooLowForPaymore : Exception
    {
        public string Code { get; set; }

        public string Details { get; set; }
    }
}