using System;
using System.Linq;
using NUnit.Framework;
using PayLess.Models;
using RestSharp;
using HttpStatusCode = System.Net.HttpStatusCode;

namespace PayLessSpecs.PayMore
{
    [TestFixture]
    public class PurchaseWithPayMoreSpcs
    {
        [Test]
        public void MakePurchase_Succeeds()
        {
			var client = new RestClient("http://localhost:51500");
            client.FollowRedirects = false;            
            var request = new RestRequest("/paymore/purchase", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new Purchase
            {
                AccountNumber = "442345678901",
                Amount = "5.00",
                Currency = "GBP",
                Location = "GB"
            });
            
            var response = client.Execute(request);
            
            Console.WriteLine("STATUS:{0}", response.StatusCode);
            Console.WriteLine("BODY:{0}", response.Content);

            Assert.That((HttpStatusCode)response.StatusCode, Is.EqualTo(HttpStatusCode.SeeOther));

            var locationHeader = response.Headers.SingleOrDefault(h => h.Name == "Location");
            Assert.That(locationHeader, Is.Not.Null);
            Assert.That(locationHeader.Value, Is.StringStarting("/paymore/purchase/"));

        }



        [Test]
        public void missingParamterReturnsBadRequestWithDetails()
        {
			var client = new RestClient("http://localhost:51500");
            client.FollowRedirects = false;

            var request = new RestRequest("/paymore/purchase", Method.POST);
            
            var response = client.Execute(request);

            Console.WriteLine("STATUS:{0}", response.StatusCode);
            Console.WriteLine("BODY:{0}", response.Content);

            Assert.That((HttpStatusCode)response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public void IncompatibleParametersReturnsForbiddenWithDetails()
        {
            var client = new RestClient("http://localhost:51500");
            client.FollowRedirects = false;

            var request = new RestRequest("/paymore/purchase", Method.POST);
            request.AddBody(new Purchase
            {
                AccountNumber = "442345",
                Amount = "5.00",
                Currency = "GBP",
                Location = "GB"
            });

            var response = client.Execute(request);

            Console.WriteLine("STATUS:{0}", response.StatusCode);
            Console.WriteLine("BODY:{0}", response.Content);

            Assert.That((HttpStatusCode)response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }

        [TestCase("4.99")]
        public void Should_throw_error_if_amount_is_less_that_5_00_for_uk(string amount)
        {
            var client = new RestClient("http://localhost:51500");
            client.FollowRedirects = false;
            var request = new RestRequest("/paymore/purchase", Method.POST);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new Purchase
            {
                AccountNumber = "442345678901",
                Amount = amount,
                Currency = "GBP",
                Location = "GB"
            });

            var response = client.Execute(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
            Assert.That(response.Content,Is.StringContaining("Minimum amount of 5.00"));
            
        }
    }
}