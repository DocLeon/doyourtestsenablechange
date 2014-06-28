using Nancy;
using Nancy.Responses;

namespace PayLess.Modules
{
    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/"] = _ => View["Index"];
        }
    }
}