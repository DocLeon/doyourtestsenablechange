using NUnit.Framework;
using PayLess;

namespace PayLessSpecs
{
	[TestFixture]
	public class CardSpecs
	{
		private PayLess.RegisteredCard _registeredCard;

		public CardSpecs()
		{
			_registeredCard = Card.Register(new CardDetails
			{
				CardType = "CARD_TYPE",
				CardNumber = "CARD_NUMBER",
			});
		}
		[Test]
		public void Should_register_card_details()
		{			 
			Assert.That(_registeredCard.Number, Is.EqualTo(_registeredCard.Number));
		}
	}
}
