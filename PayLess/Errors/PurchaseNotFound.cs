using System;

namespace PayLess.Errors
{
    public class PurchaseNotFound : Exception
    {
        public PurchaseNotFound(string message) : base(message){}
    }
}