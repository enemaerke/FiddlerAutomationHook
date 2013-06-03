using System.Web;
using Funq;
using ServiceStack.Common;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.Text;
using ServiceStack.WebHost.Endpoints;

namespace FiddlerAutomationHook
{
  public class ServiceStackAppHost : AppHostHttpListenerBase
  {
    private readonly SegmentEvaluator m_evaluator;

    public ServiceStackAppHost(SegmentEvaluator evaluator)
      :base("Automation Hook", typeof(ServiceStackAppHost).Assembly)
    {
      m_evaluator = evaluator;
    }

    public override void Configure(Container container)
    {
      //json setup
      JsConfig.EmitCamelCaseNames = true;
      JsConfig.IncludeNullValues = false;
      JsConfig.PropertyConvention = JsonPropertyConvention.Lenient;

      bool debugMode = false;
#if DEBUG
      debugMode = true;
#endif

      container.Register<SegmentEvaluator>(c => m_evaluator);

      //feature configuration
      const Feature disableFeatures = Feature.Jsv | Feature.Soap;
      SetConfig(new EndpointHostConfig
        {
          EnableFeatures = Feature.All.Remove(disableFeatures), //all formats except of JSV and SOAP
          DebugMode = debugMode, //Show StackTraces in service responses during development
          WriteErrorsToResponse = false, //Disable exception handling
          DefaultContentType = ContentType.Json, //Change default content type
          AllowJsonpRequests = false //Enable JSONP requests
        });

      //var conf = new EndpointHostConfig()
      //{
      //    DefaultRedirectPath = "/info",
      //};

      //conf.RawHttpHandlers.Add(r =>
      //{
      //    if (r.RawUrl == "/")
      //    {
      //        HttpContext.Current.RewritePath(conf.DefaultRedirectPath);
      //    }

      //    return null;
      //});

      //this.SetConfig(conf);
    }
  }
}