using Nancy;

namespace PayLess
{
	public class CardDetails
	{
		public static CardDetails From(Request request)
		{
			return new CardDetails
				       {
					       CardType = request.Form["cardtype"]
				       };
		}

		public string CardType { get; set; }
	}
}