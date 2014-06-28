using Nancy;
using Nancy.Conventions;

namespace PayLess
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            StaticConfiguration.DisableErrorTraces = false;
            nancyConventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("static", @"Static"));
            nancyConventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddFile("favicon.png", "static/favicon.png"));

            base.ConfigureConventions(nancyConventions);
        }
    }
}