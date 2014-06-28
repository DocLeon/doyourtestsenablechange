using System;
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
    }
}