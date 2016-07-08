using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAGBOSS.Common
{
  public class TAGClock
  {
    DateTime startTime = new DateTime();
    DateTime endTime = new DateTime();
    TimeSpan interval = new TimeSpan(0);
    bool isRunning = false;

    public double ElapsedSeconds { get { return Convert.ToDouble(interval.TotalSeconds); } }
    public DateTime StartTime { get { return startTime; } }
    public DateTime EndTime { get { return endTime; } }
    public bool IsRunning { get { return isRunning; } }
    public double ElapsedMilliseconds { get { return interval.TotalMilliseconds; } }

    public void Start()
    {
      startTime = DateTime.Now;
      isRunning = true;
      interval = new TimeSpan(0);
    }
    public void Stop()
    {
      endTime = DateTime.Now;
      isRunning = false;
      interval = endTime - startTime;
    }
  }
}
