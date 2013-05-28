using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.ServiceHost;

namespace FiddlerAutomationHook.Services
{
  [Route("/status", "GET")]
  public class StatusRequest : IReturn<StatusResponse>
  {
  }

  public class StatusResponse
  {
    public List<SegmentMatcher> Matchers { get; set; }
  }

  [Route("/matchers", "PUT, DELETE")]
  public class SegmentMatcherRequest : IReturn<Result>
  {
    public string SegmentMatcher { get; set; }
  }

  public class Result
  {
    public bool Success { get; set; }
  }

  [Route("/results", "GET")]
  public class MatchResultRequest: IReturn<MatchResultResponse>
  {
    public string SegmentMatcher { get; set; }
  }

  public class MatchResultResponse
  {
    public Match[] Results { get; set; }
  }
}
