using System.Text;
using Nancy;
using Nancy.Bootstrapper;

namespace PayLess
{
	public class InvalidDetailsError: IApplicationStartup
	{
		public void Initialize(IPipelines pipelines)
		{
			pipelines.OnError += (context, exception) =>
				                     {
					                     if (exception is InvalidCardDetails)
					                     {
						                     return new Response
							                            {
								                            StatusCode = HttpStatusCode.BadRequest,
								                            ContentType = "text/plain",
								                            Contents = (stream) =>
									                                       {
										                                       var errorMessage = Encoding.UTF8.GetBytes(exception.Message);
										                                       stream.Write(errorMessage, 0, errorMessage.Length);
									                                       }
							                            };
					                     }
					                     return HttpStatusCode.InternalServerError;
				                     };
		}
	}
}