using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Fiddler;

namespace FiddlerAutomationHook
{
  public class SegmentMatcherExtension : IAutoTamper
  {
    private readonly SegmentEvaluator m_evaluator = new SegmentEvaluator();
    private ServiceStackAppHost m_appHost;
    public SegmentMatcherExtension()
    {
    }
    public void OnLoad()
    {
      try
      {
        var listeningOn = "http://*:7676/";
        m_appHost = new ServiceStackAppHost(m_evaluator);
        m_appHost.Init();
        m_appHost.Start(listeningOn);
        FiddlerApplication.Log.LogString("Started AutomationHook");
      }
      catch (Exception exc)
      {
        FiddlerApplication.Log.LogString("FiddlerAutomationHook. Exception caught in OnLoad:" + exc.FullExceptionDetails());
      }
    }

    public void OnBeforeUnload()
    {
      try
      {
        if (m_appHost != null)
          m_appHost.Stop();
      }
      catch (Exception exc)
      {
        FiddlerApplication.Log.LogString("FiddlerAutomationHook. Exception caught in OnBeforeUnload:" + exc.Message);
      }
    }

    public void AutoTamperRequestBefore(Session oSession)
    {
      m_evaluator.Evaluate(oSession);
    }

    public void AutoTamperRequestAfter(Session oSession)
    {
    }

    public void AutoTamperResponseBefore(Session oSession)
    {
    }

    public void AutoTamperResponseAfter(Session oSession)
    {
    }

    public void OnBeforeReturningError(Session oSession)
    {
    }
  }
}
