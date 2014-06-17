using Nancy;
using Nancy.ModelBinding;

namespace PayLess
{
	public class PayLessModule : NancyModule
	{
		private readonly IObscureCardNumber _cardNumberObscurer;
		private IRegisterCards _card;

		public PayLessModule(IObscureCardNumber cardNumberObscurer, IRegisterCards card)
		{
			_cardNumberObscurer = cardNumberObscurer;
			_card = card;
			Post["payless/cardregistration"] = parameters =>
				                                   {
					                                   var cardDetails = this.Bind<CardDetails>();
													   cardDetails.CardNumber = _cardNumberObscurer.Obscure(cardDetails.CardNumber);
					                                   return Response.AsJson
						                                   (_card.Register(cardDetails),
						                                    HttpStatusCode.Created);
				                                   };
		}
	}

	public interface IRegisterCards
	{
		RegisteredCard Register(CardDetails cardDetails);
	}
}