using System.Collections.Generic;

namespace FiddlerAutomationHook
{
  public class SegmentMatcher
  {
    public string MatchClause { get; set; }
    public List<Match> Matches { get; set; }

    internal void AddMatch(Match match, int lowWaterMark, int highWaterMark)
    {
      Matches.Add(match);
      if (Matches.Count > highWaterMark)
        Matches.RemoveRange(0, highWaterMark-lowWaterMark);
    }

    internal SegmentMatcher Copy()
    {
      return new SegmentMatcher()
        {
          MatchClause = MatchClause,
          Matches = new List<Match>(Matches)
        };
    }
  }
}