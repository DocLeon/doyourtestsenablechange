using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PayLess;

namespace PayLessSpecs
{
	[TestFixture]
	public class CardStoreSpec
	{
		[Test]
		public void should_insert_card_and_return_identifier()
		{
			var cardStore = new CardStore();
			var id = cardStore.Save(new CardDetails());
			Assert.That(id,Is.Not.Null);
		}
	}
}
