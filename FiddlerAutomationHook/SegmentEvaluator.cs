using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Fiddler;

namespace FiddlerAutomationHook
{
  public class SegmentEvaluator
  {
    private readonly ReaderWriterLockSlim m_lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
    private readonly Dictionary<string, SegmentMatcher> m_segmentMatchers = new Dictionary<string, SegmentMatcher>();
    private bool m_isEnabled = false;

    public SegmentEvaluator()
    {
      HighWaterMark = 100;
      LowWaterMark = 50;
    }

    internal void Reset()
    {
      using (m_lock.WriteLock())
      {
        foreach (var c in m_segmentMatchers)
          m_segmentMatchers.Clear();

        m_isEnabled = false;
      }
    }

    internal void ClearSegmentMatchers()
    {
      using (m_lock.WriteLock())
      {
        m_segmentMatchers.Clear();

        m_isEnabled = false;
      }
    }

    internal void AddSegmentMatch(string urlSegment)
    {
      using (m_lock.WriteLock())
      {
        if (!m_segmentMatchers.ContainsKey(urlSegment))
          m_segmentMatchers[urlSegment] = new SegmentMatcher()
            {
              MatchClause = urlSegment,
              Matches = new List<Match>(),
            };
        m_isEnabled = true;
      }
    }

    internal void Evaluate(Session session)
    {
      if (m_isEnabled)
      {
        using (m_lock.ReadLock())
        {
          m_segmentMatchers
            .Where(x => session.uriContains(x.Key))
            .ForEach(x => x.Value.AddMatch(
              new Match(){
                Url = session.url,
                Data =session.requestBodyBytes
            },
            LowWaterMark, HighWaterMark
            ));
        }
      }
    }

    protected int LowWaterMark { get; set; }
    protected int HighWaterMark { get; set; }

    public List<SegmentMatcher> GetMatchers()
    {
      using (m_lock.ReadLock())
      {
        return m_segmentMatchers.Select(x => x.Value.Copy()).ToList();
      }
    }

    public bool RemoveSegmentMatcher(string segmentMatcher)
    {
      using (m_lock.WriteLock())
      {
        return m_segmentMatchers.Remove(segmentMatcher);
      }
    }

    public Match[] GetMatchResults(string segmentMatcher)
    {
      if (!string.IsNullOrEmpty(segmentMatcher))
      {
        using (m_lock.ReadLock())
        {
          SegmentMatcher matcher;
          if (m_segmentMatchers.TryGetValue(segmentMatcher, out matcher))
            return matcher.Matches.ToArray();
        }
      }
      return new Match[0];
    }
  }
}
