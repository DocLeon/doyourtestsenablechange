using System.Net;
using NUnit.Framework;
using RestSharp;

namespace PayLessSpecs
{
	[TestFixture]
	public class CardRegistration
	{
		[Test]
		public void post_should_create_card_registration()
		{
			var client = new RestClient("http://localhost:51500/payless/");
			var request = new RestRequest("cardregistration/",Method.POST);
			request.AddParameter("cardtype", "Maestro");
			request.AddParameter("cardnumber", "5454545454545454");
			request.AddParameter("cardholdername", "CARDHOLDERNAME");			
			request.AddParameter("startdate", "01/14");
			request.AddParameter("expirydate", "01/20");
			request.AddParameter("CVV", "123");
			request.AddParameter("issuenumber", "1");

			var response = client.Execute(request);
			Assert.That(response.ErrorException, Is.Null,"Request received with no problems");
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created),"Registration created");
			Assert.That(response.Content,Contains.Substring(@"""type"":""Maestro"""),"Card Type");
			Assert.That(response.Content,Contains.Substring(@"""number"":""************5454"""),"Card number");
			Assert.That(response.Content,Contains.Substring(@"""expiryDate"":""01/20"""), "Expiry Date");
			Assert.That(response.Content,Contains.Substring(@"""cardHolderName"":""CARDHOLDERNAME"""));
		}
	}
}
