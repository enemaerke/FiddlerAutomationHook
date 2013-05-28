using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FiddlerAutomationHook;
using FiddlerAutomationHook.Services;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using ServiceStack.Text;

namespace TestHarness
{
  class Program
  {
    static void Main(string[] args)
    {
      //FiddlerAutomationHook.SegmentEvaluator evaluator = new SegmentEvaluator();
      //FiddlerAutomationHook.ServiceStackAppHost appHost = new ServiceStackAppHost(evaluator);
      //var listeningOn = "http://*:7676/";
      //appHost.Init();
      //appHost.Start(listeningOn);

      ServiceStack.Service.IServiceClient client = new JsonServiceClient("http://localhost:7676");
      GetStatus(client);

      PuSegmentMatcher(client, "http://google.com");

      GetStatus(client);

      Console.WriteLine("Done, press enter to exit");
      Console.ReadLine();
    }

    private static void PuSegmentMatcher(IServiceClient client, string segment)
    {

      client.PutAsync(new SegmentMatcherRequest()
      {
        SegmentMatcher = segment,
      }, (r) =>
      {
        Console.WriteLine("Result of add:" + r.Success);
      }, (r, e) =>
      {
        Console.WriteLine("Error adding:" + e.Dump());
      });
      Wait();
    }

    private static void GetStatus(IServiceClient client)
    {
      client.GetAsync(new StatusRequest(),
        (r) =>
        {
          Console.WriteLine("Result of status:" + r.Dump());
        }, (r, e) =>
        {
          Console.WriteLine("Error adding:" + e.Dump());
        });
      Wait();
    }

    private static void Wait()
    {
      Console.ReadLine();
    }
  }
}
