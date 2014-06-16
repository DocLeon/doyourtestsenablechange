using Nancy;

namespace PayLess
{
	public class PayLessModule : NancyModule
	{
		public PayLessModule()
		{
			Post["payless/cardregistration"] = parameters => Response.AsJson
				                                                 (Card.Register(CardDetails.From(Request)),
				                                                  HttpStatusCode.Created);

		}
	}
}