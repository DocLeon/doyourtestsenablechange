using System.Text;
using Nancy;
using Nancy.Bootstrapper;

namespace PayLess
{
	public class ErrorHandling: IApplicationStartup
	{
		public void Initialize(IPipelines pipelines)
		{
			pipelines.OnError += (context, exception) =>
				                     {
					                     if (exception is missingParameterException)
					                     {
						                     return BuildResponse(string.Format("ERROR:{0} {1} missing",
								                                                          (exception as missingParameterException).Code,
								                                                          (exception as missingParameterException).Parameter),
																						  HttpStatusCode.BadRequest);											 
					                     }
										 if (exception is MissingValueException)
											 return BuildResponse(string.Format("ERROR:{0} {1} not supplied",
											                                               (exception as MissingValueException).Code,
											                                               (exception as MissingValueException).Parameter), 
																						    HttpStatusCode.BadRequest);
					                     if (exception is InvalidPurchaseMade)
						                     return BuildResponse(string.Format("ERROR:{0} {1}",
						                                                        (exception as InvalidPurchaseMade).ErrorCode,
						                                                        (exception as InvalidPurchaseMade).Reason),
																				HttpStatusCode.Forbidden);
					                     return HttpStatusCode.InternalServerError;
				                     };			
		}

		private static Response BuildResponse(string message, HttpStatusCode statusCode)
		{
			return new Response
				       {
					       StatusCode = statusCode,
					       ContentType = "text/plain",
					       Contents = (stream) =>
						                  {
							                  var errorMessage =
								                  Encoding.UTF8.GetBytes(message);
							                  stream.Write(errorMessage, 0, errorMessage.Length);
						                  }
				       };
		}
	}
}