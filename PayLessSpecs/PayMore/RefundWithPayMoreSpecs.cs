using System;
using System.Linq;
using Nancy;
using NUnit.Framework;
using PayLess.Models;
using RestSharp;

namespace PayLessSpecs.PayMore
{
    [TestFixture]
    public class RefundWithPayMoreSpecs
    {
        [Test]
        public void Should_return_not_allowed_for_purchase_root()
        {
            var client = new RestClient("http://localhost:51500");
            client.FollowRedirects = false;
            var request = new RestRequest("/paymore/purchase", Method.DELETE);
            
            var response = client.Execute(request);

            Assert.That((HttpStatusCode)response.StatusCode, Is.EqualTo(HttpStatusCode.MethodNotAllowed));            
        }

        [Test]
        public void Should_return_not_found_when_resource_doesnt_exist()
        {
            var purchaseId = "-bad-id";


            var client = new RestClient("http://localhost:51500");
            client.FollowRedirects = false;


            var request = new RestRequest(string.Format("/paymore/purchase/{0}", purchaseId), Method.DELETE);

            var response = client.Execute(request);

            Assert.That((HttpStatusCode)response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));                        
        }

        [Test]
        public void Can_delete_existing_purchase()
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

            var deletePurchaseResource = response.Headers.SingleOrDefault(h => h.Name == "Location");
            var deleteRequest = new RestRequest(deletePurchaseResource.Value.ToString(), Method.DELETE);
            var deleteResponse = client.Execute(deleteRequest);

            Console.WriteLine("STATUS:{0}", response.StatusCode);
            Console.WriteLine("BODY:{0}", response.Content);

            Assert.That(deleteResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));


        }
    }
}