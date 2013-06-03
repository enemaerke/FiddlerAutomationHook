using System.Collections.Generic;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.Text;

namespace FiddlerAutomationHook.Services
{
  public class SegmentMatchService : IService
  {
    private readonly SegmentEvaluator m_evaluator;

    public SegmentMatchService(SegmentEvaluator evaluator)
    {
      m_evaluator = evaluator;
    }

    public InfoResponse Get(InfoRequest request)
    {
      return new InfoResponse()
        {
          Info = "Version {0} of AutomationHook".Fmt(GetType().Assembly.GetName().Version.ToString()),
        };
    }

    public StatusResponse Get(StatusRequest request)
    {
      var matchers = m_evaluator.GetMatchers();
      return new StatusResponse()
        {
          Matchers = matchers
        };
    }

    public Result Put(SegmentMatcherRequest request)
    {
      bool success = false;
      if (!string.IsNullOrEmpty(request.SegmentMatcher))
      {
        m_evaluator.AddSegmentMatch(request.SegmentMatcher);
        success = true;
      }
      return new Result()
      {
        Success = success
      };
    }

    public Result Delete(SegmentMatcherRequest request)
    {
      bool success = true;
      if (request.SegmentMatcher == null)
        m_evaluator.ClearSegmentMatchers();
      else
        success = m_evaluator.RemoveSegmentMatcher(request.SegmentMatcher);
      return new Result()
      {
        Success = success
      };
    }

    public MatchResultResponse Get(MatchResultRequest request)
    {
      var results = m_evaluator.GetMatchResults(request.SegmentMatcher);
      return new MatchResultResponse()
        {
          Results = results
        };
    }
  }
}