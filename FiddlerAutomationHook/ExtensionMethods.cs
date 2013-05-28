using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ServiceStack.Text;

namespace FiddlerAutomationHook
{
  internal static class ExtensionMethods
  {
    internal class ActionDisposable : IDisposable
    {
      private Action m_disposeAction;

      internal ActionDisposable(Action disposeAction)
      {
        m_disposeAction = disposeAction;
      }

      public void Dispose()
      {
        m_disposeAction();
        m_disposeAction = null;
      }
    }

    internal static IDisposable ReadLock(this ReaderWriterLockSlim readerWriterLockSlimlock)
    {
      readerWriterLockSlimlock.EnterReadLock();
      return new ActionDisposable(readerWriterLockSlimlock.ExitReadLock);
    }

    internal static IDisposable WriteLock(this ReaderWriterLockSlim readerWriterLockSlimlock)
    {
      readerWriterLockSlimlock.EnterWriteLock();
      return new ActionDisposable(readerWriterLockSlimlock.ExitWriteLock);
    }

    internal static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
      if (enumerable != null && action != null)
      {
        foreach (var t in enumerable)
          action(t);
      }
    }

    internal static string FullExceptionDetails(this Exception exc, int indent = 0)
    {
      StringBuilder stringBuilder= new StringBuilder();
      stringBuilder.AppendLine("{2}{0} : {1}".Fmt(exc.GetType().Name, exc.Message, "".PadRight(indent, '#')));
      stringBuilder.Append(exc.StackTrace);
      if (exc.InnerException != null)
        stringBuilder.Append(exc.InnerException.FullExceptionDetails(indent + 2));
      return stringBuilder.ToString();
    }
  }
}