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
        //private string payMoreBaseUrl = "http://www.doyourtestsenablechange.com";
        private string payMoreBaseUrl = "http://localhost:51500";

        [Test]
        public void Should_return_not_allowed_for_purchase_root()
        {
            var client = new RestClient(payMoreBaseUrl);
            client.FollowRedirects = false;
            var request = new RestRequest("/paymore/purchase", Method.DELETE);
            
            var response = client.Execute(request);

            Assert.That((HttpStatusCode)response.StatusCode, Is.EqualTo(HttpStatusCode.MethodNotAllowed));            
        }

        [Test]
        public void Should_return_not_found_when_resource_doesnt_exist()
        {
            var purchaseId = "-bad-id";


            var client = new RestClient(payMoreBaseUrl);
            client.FollowRedirects = false;


            var request = new RestRequest(string.Format("/paymore/purchase/{0}", purchaseId), Method.DELETE);

            var response = client.Execute(request);

            Assert.That((HttpStatusCode)response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));                        
        }

        [Test]
        public void Can_delete_existing_purchase()
        {
            var client = new RestClient(payMoreBaseUrl);
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

        [Test]
        public void Should_return_NotFound_when_deleting_a_resource_that_isnt_there()
        {
            var client = new RestClient(payMoreBaseUrl);
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

            var purchaseThatDoesntExist = Guid.NewGuid().ToString();
            var deleteRequest = new RestRequest("/paymore/purchase/"+purchaseThatDoesntExist, Method.DELETE);
            var deleteResponse = client.Execute(deleteRequest);

            Console.WriteLine("STATUS:{0}", response.StatusCode);
            Console.WriteLine("BODY:{0}", response.Content);

            Assert.That(deleteResponse.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.NotFound));
        }

        [Test]
        public void Should_return_Purchase_resource_when_it_exists()
        {
            var client = new RestClient(payMoreBaseUrl);
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

            var purchaseResource = response.Headers.SingleOrDefault(h => h.Name == "Location");
            var getPurchaseResource = new RestRequest(purchaseResource.Value.ToString(), Method.GET);
            var purchaseReource = client.Execute<Purchase>(getPurchaseResource);

            Console.WriteLine("STATUS:{0}", response.StatusCode);
            Console.WriteLine("BODY:{0}", response.Content);

            Assert.That(purchaseReource.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
            
        }
    }
}