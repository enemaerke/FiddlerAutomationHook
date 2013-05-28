using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fiddler;

namespace FiddlerAutomationHook
{
  public class SimpleExtension : IAutoTamper
  {
    public void OnLoad()
    {
    }

    public void OnBeforeUnload()
    {
    }

    public void AutoTamperRequestBefore(Session oSession)
    {
      oSession.oRequest["User-Agent"] = "Violin";
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
