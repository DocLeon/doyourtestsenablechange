using System;

namespace PayLess.Modules
{
    public class PurchaseNotFound : Exception
    {
        public PurchaseNotFound(string message) : base(message){}
    }
}