using Nancy;
using Nancy.ModelBinding;

namespace PayLess
{
	public class PayLessModule : NancyModule
	{
		public PayLessModule()
		{
			Post["payless/cardregistration"] = parameters =>
				                                   {
					                                   var cardDetails = this.Bind<CardDetails>();
					                                   return Response.AsJson
						                                   (Card.Register(cardDetails),
						                                    HttpStatusCode.Created);
				                                   };

		}
	}
}