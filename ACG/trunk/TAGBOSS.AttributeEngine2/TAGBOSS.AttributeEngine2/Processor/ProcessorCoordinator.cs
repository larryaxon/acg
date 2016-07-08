using System;
using System.Collections;
using System.Threading;

using TAGBOSS.Common.Model;

namespace TAGBOSS.Sys.AttributeEngine2.Processor
{
  public sealed class ProcessorCoordinator
  {
    private ArrayList includeQueue = new ArrayList();
    private ArrayList referenceQueue = new ArrayList();
    private ArrayList conditionalQueue = new ArrayList();

    private int threads = 0;

    public ProcessorCoordinator()
    {
    }

    //Here is the include queue processor
    public void QueueInclude(ProcessorDelegate processor)
    {
      includeQueue.Add(processor);
    }

    public void StartInclude(int blockSize)
    {
      Thread thread;
      int r = (includeQueue.Count % blockSize);
      int c = (includeQueue.Count / blockSize) + (r > 0 ? 1 : 0);

      for (int j = 0; j < c; j++) 
      {
        threads++;
        thread = new Thread(new ParameterizedThreadStart(ProcessQueueInclude));
        thread.IsBackground = true;
        thread.Start(new ProcessQueueParameters(j * blockSize, blockSize));
      }
      WaitQueueProcessing();
      includeQueue.Clear();
    }

    private void ProcessQueueInclude(object p) 
    {
      try
      {
        int begin = ((ProcessQueueParameters)p).begin, length = ((ProcessQueueParameters)p).length;

        if (begin + length > includeQueue.Count)
          length = includeQueue.Count - begin;

        for (int i = 0; i < length; i++)
          ((ProcessorDelegate)includeQueue[begin + i]).Process();
      }
      catch (Exception e)
      {
        //TODO: This must NEVER happen! but...
        //If it happens, ADD to Log file
        Console.WriteLine(e.Message);
        Console.WriteLine(e.StackTrace);
      }
      finally 
      {
        threads--;
      }
    }

    //Here is the reference queue processor
    public void QueueReference(ProcessorDelegate processor)
    {
      referenceQueue.Add(processor);
    }

    public void StartReference(int blockSize)
    {
      Thread thread;
      int r = (referenceQueue.Count % blockSize);
      int c = (referenceQueue.Count / blockSize) + (r > 0 ? 1 : 0);

      for (int j = 0; j < c; j++)
      {
        threads++;
        thread = new Thread(new ParameterizedThreadStart(ProcessQueueReference));
        thread.IsBackground = true;
        thread.Start(new ProcessQueueParameters(j * blockSize, blockSize));
      }
      WaitQueueProcessing();
      referenceQueue.Clear();
    }

    private void ProcessQueueReference(object p)
    {
      try
      {
        int begin = ((ProcessQueueParameters)p).begin, length = ((ProcessQueueParameters)p).length;

        if (begin + length > referenceQueue.Count)
          length = referenceQueue.Count - begin;

        for (int i = 0; i < length; i++)
          ((ProcessorDelegate)referenceQueue[begin + i]).Process();
      }
      catch (Exception e)
      {
        //TODO: This must NEVER happen! but...
        //If it happens, ADD to Log file
        Console.WriteLine(e.Message);
        Console.WriteLine(e.StackTrace);
      }
      finally
      {
        threads--;
      }
    }

    //Here is the reference queue processor
    public void QueueConditional(ProcessorDelegate processor)
    {
      conditionalQueue.Add(processor);
    }

    public void StartConditional(int blockSize)
    {
      Thread thread;
      int r = (conditionalQueue.Count % blockSize);
      int c = (conditionalQueue.Count / blockSize) + (r > 0 ? 1 : 0);

      for (int j = 0; j < c; j++)
      {
        threads++;
        thread = new Thread(new ParameterizedThreadStart(ProcessQueueConditional));
        thread.IsBackground = true;
        thread.Start(new ProcessQueueParameters(j * blockSize, blockSize));
      }
      WaitQueueProcessing();
      conditionalQueue.Clear();
    }

    private void ProcessQueueConditional(object p)
    {
      try
      {
        int begin = ((ProcessQueueParameters)p).begin, length = ((ProcessQueueParameters)p).length;

        if (begin + length > conditionalQueue.Count)
          length = conditionalQueue.Count - begin;

        for (int i = 0; i < length; i++)
          ((ProcessorDelegate)conditionalQueue[begin + i]).Process();
      }
      catch (Exception e)
      {
        //TODO: This must NEVER happen! but...
        //If it happens, ADD to Log file
        Console.WriteLine(e.Message);
        Console.WriteLine(e.StackTrace);
      }
      finally
      {
        threads--;
      }
    }

    //For now we only WAIT for the includeQueue to be processed, before we call the conditionalQueue processor
    public void WaitQueueProcessing()
    {
      while (threads > 0)
        Thread.Sleep(Constants.ThreadSleepTime);
    }

    struct ProcessQueueParameters 
    {
      public int begin;
      public int length;

      public ProcessQueueParameters(int begin, int length) 
      {
        this.begin = begin;
        this.length = length;
      }
    }
  }
}