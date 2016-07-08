using System;

using TAGBOSS.Sys.AttributeEngine2.Dal;
using TAGBOSS.Sys.AttributeEngine2.Model;
using TAGBOSS.Sys.AttributeEngine2.Processor;
using TAGBOSS.Sys.AttributeEngine2.Common;

namespace Test_AtributeEngine2
{
  class Program
  {
    static void Main(string[] args)
    {
      //TAGBOSS.Common.TAGFunctions2 TAGFunctions = new TAGBOSS.Common.TAGFunctions2();

      Console.WriteLine("foo1:" + DateTime.Now);
      
      SystemEntityFactory.Instance.GetType();
      AttributeData ad = new AttributeData();
      
      Console.WriteLine("foo2:" + DateTime.Now);
      TEntity[] eList = ad.Entities(DateTime.Now);
      Console.WriteLine("bar:" + DateTime.Now);
    }
  }
}
