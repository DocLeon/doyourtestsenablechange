using Nancy;
using Nancy.ModelBinding;

namespace PayLess
{
	public class PayLessModule : NancyModule
	{
		private readonly IObscureCardNumber _cardNumberObscurer;

		public PayLessModule(IObscureCardNumber cardNumberObscurer)
		{
			_cardNumberObscurer = cardNumberObscurer;
			Post["payless/cardregistration"] = parameters =>
				                                   {
					                                   var cardDetails = this.Bind<CardDetails>();
													   cardDetails.CardNumber = _cardNumberObscurer.Obscure(cardDetails.CardNumber);
					                                   return Response.AsJson
						                                   (Card.Register(cardDetails),
						                                    HttpStatusCode.Created);
				                                   };
		}
	}
}