using System.Collections.Generic;

namespace PayLess
{
	public class PayLessValidation: IValidatePurchaseCanBeBuilt
	{
		private IParseQueryStrings _fields;

		private readonly IDictionary<string,string> _compulsoryFields = new Dictionary<string, string>
				                                     {
					                                     {"accountnumber", "1234"},
					                                     {"location", "UK"},
					                                     {"amount", "1.99"},
					                                     {"currency", "GBP"}
				                                     };

		public PayLessValidation(IParseQueryStrings fields)
		{
			_fields = fields;
		}

		public void CanBuildPurchaseFrom(string purchaseParams)
		{
			var fields = _fields.Parse(purchaseParams);
			foreach (var field in _compulsoryFields.Keys)
				if (!fields.ContainsKey(field))
					throw new missingParameterException
						      {
							      Code = 3000 + field.Length,
								  Parameter = field
						      };
		}
	}
}