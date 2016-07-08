using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAGBOSS.Common;
using TAGBOSS.Common.Model;

namespace TAGBOSS.Common
{
  public sealed class DictionaryFactory 
  {
    const string cDICTIONARY = "Dictionary";      // TAGAttribute Entity for dictionary entries

    public static readonly DictionaryFactory dictionaryFactoryInstance = new DictionaryFactory();
    private static Dictionaries dictionary = null;
    private DateTime lastDateTime = DateTime.Now;
    public DateTime LastDateTimeRefreshed { get { return lastDateTime; } }
    public static DictionaryFactory getInstance()
    {
      return dictionaryFactoryInstance;
    }
    public Dictionaries getDictionary()
    {
      return dictionary;
    }
    public void setDictionary(Dictionaries dict)
    {
      dictionary = dict;
      lastDateTime = DateTime.Now;
    }

  }
}
