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
						                     return new Response
							                            {
								                            StatusCode = HttpStatusCode.BadRequest,
								                            ContentType = "text/plain",
								                            Contents = (stream) =>
									                                       {
										                                       var errorMessage =
											                                       Encoding.UTF8.GetBytes(string.Format("ERROR:{0} {1} missing",(exception as missingParameterException).Code,(exception as missingParameterException).Parameter));
										                                       stream.Write(errorMessage, 0, errorMessage.Length);
									                                       }
							                            };
					                     }
					                     return HttpStatusCode.InternalServerError;
				                     };			
		}
	}
}