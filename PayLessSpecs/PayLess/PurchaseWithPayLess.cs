using System;
using System.Net;
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
			var client = new RestClient("http://localhost:51500");
			var request = new RestRequest("/payless/makepurchase?accountnumber=441234567890&location=GB&amount=1.99&currency=GBP", Method.POST);

			var response = client.Execute(request);
			Console.WriteLine("STATUS:{0}",response.StatusCode);
			Console.WriteLine("BODY:{0}", response.Content);
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
		}

		[Test]
		public void missingParamterReturnsBadRequestWithDetails()
		{			
		}

		[Test]
		public void IncompatibleParametersReturnsForbiddenWithDetails()
		{			
		}
	}
}
