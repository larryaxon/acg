using System;
using System.Runtime.InteropServices;

using TAGBOSS.Common.Logging;

namespace TAGBOSS.Common
{
  [Serializable]
  public class FunctionExceptionHandler: IDisposable
  {
    Log log = (Log)LogFactory.GetInstance().GetLog("TAGBOSS.Common.FunctionExceptionHandler");

    public Exception Exception { get; set; }

    #region IDisposable Members

    void IDisposable.Dispose()
    {
      //Console.WriteLine("testing");
      if (Marshal.GetExceptionCode() != 0 || this.Exception != null)
      {
        if (Exception != null)
          log.Warn("Life Hurts!!!", Exception);
        //Console.WriteLine("Life Hurts!!!");
        //Console.WriteLine(this.Exception.Message);
        //Console.WriteLine(this.Exception.StackTrace);
      }
    }

    #endregion
  }
}
