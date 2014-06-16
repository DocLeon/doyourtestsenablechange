using Nancy;

namespace PayLess
{
	public class PayLessModule : NancyModule
	{
		public PayLessModule()
		{
			Post["payless/cardregistration"] = parameters => Response.AsJson(new CardDetails {Type = "Maestro"},
					                                                                  HttpStatusCode.Created);
					                               
		}
	}

	public class CardDetails
	{
		public string Type { get; set; }
	}
}