using System;

namespace TAGBOSS.Common.Model
{
  public delegate void ProcessRun();

  public class ProcessorDelegate
  {
    public ProcessRun Process;
  }
}
