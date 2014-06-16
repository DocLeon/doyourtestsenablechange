using NUnit.Framework;
using Nancy.Testing;
using PayLess;

namespace PayLessSpecs
{
	[TestFixture]
	public class ModuleSpec
	{
		[Test]
		public void Should_return_registered_card()
		{
			
			var browser = new Browser(with => with.Module<PayLessModule>());

			var response = browser.Post("payless/cardregistration", 
											with =>
												{
													with.HttpRequest();
													with.FormValue("cardtype","CARD-TYPE");
												}
											);
			
			Assert.That(response.Body.DeserializeJson<RegisteredCard>().Type,Is.EqualTo("CARD-TYPE"),"ResponseBody:{0}",response.Body.AsString());		
		}
	}

	public class RegisteredCard
	{
		public string Type { get; set; }
	}
}
