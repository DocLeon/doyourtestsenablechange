using System;
using NUnit.Framework;
using RestSharp;

namespace PayLessSpecs
{
	[TestFixture]
	public class PurchaseWithPayLess
	{
		[Test]
		public void MakePurchase()
		{
			var client = new RestClient("http://localhost/payless");
			var request = new RestRequest("/makepurchase?accountnumber=1234&country=GB&amount=1.99&currency=GBP", Method.POST);

			var response = client.Execute(request);
			Console.WriteLine("STATUS:{0}",response.StatusCode);
			Console.WriteLine("BODY:{0}", response.Content);
		}

		[Test]
		public void MissingParamterReturnsBadRequestWithDetails()
		{			
		}

		[Test]
		public void IncompatibleParametersReturnsForbiddenWithDetails()
		{			
		}
	}
}
