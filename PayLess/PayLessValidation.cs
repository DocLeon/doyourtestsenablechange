using System.Collections.Generic;

namespace PayLess
{
	public class PayLessValidation: IValidatePurchaseCanBeBuilt
	{
		private IParseQueryStrings _queryString;

		private readonly IDictionary<string,string> _compulsoryFields = new Dictionary<string, string>
				                                     {
					                                     {"accountnumber", "1234"},
					                                     {"location", "UK"},
					                                     {"amount", "1.99"},
					                                     {"currency", "GBP"}
				                                     };

		public PayLessValidation(IParseQueryStrings queryString)
		{
			_queryString = queryString;
		}

		public void CanBuildPurchaseFrom(string purchaseParams, IDictionary<string, string> purchaseFields)
		{
			var fields = purchaseFields;
			foreach (var field in _compulsoryFields.Keys)
			{
				if (!fields.ContainsKey(field))
					throw new missingParameterException
					{
						Code = 3000 + field.Length,
						Parameter = field
					};
				if (string.IsNullOrEmpty(fields[field]))
					throw new MissingValueException
						      {
							      Parameter = field,
							      Code = 4000 + field.Length
						      };
			}
				
		}
	}
}