using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

using BibleVerses.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BibleVerses.Manager
{
  public class VerseProcessor : IDisposable
  {
    public void Dispose() { }
    public IEnumerable<BibleVerseModel> GetBibleVerses(string username, string group)
    {
      return DataSource.GetBibleVerses(username, group);
    }
    public BibleVerseModel GetNextVerse(string username, string group)
    {
      BibleVerseModel verse = null;
      int lastVerseID = DataSource.GetLastVerseDisplayed(username, group);
      IEnumerable<BibleVerseModel> verses = DataSource.GetBibleVerses(username, group);
      if (verses != null && verses.Count() > 0)
      {
        // now fine the next verse
        if (lastVerseID == -1 || lastVerseID == verses.Count() ) // no last verse or the last one was the last verse in the sequence
          verse = verses.FirstOrDefault(); //return the first one.
        else
        {
          bool versefound = false;
          foreach (BibleVerseModel v in verses)
          {
            if (versefound)
            {
              verse = v;
              break;
            }
            if (v.ID == lastVerseID)
              versefound = true;
          }
          if ((versefound && verse == null)  ||// last verse is the LAST verse :-)
           (verse == null && verses.Count() > 0))
            verse = verses.FirstOrDefault();
        }
      }
      if (verse != null)
        DataSource.updateLastVerseDisplayed(username, group, verse.ID);
      else
        verse = new BibleVerseModel();
      return verse;
    }
    public int SaveVerse(BibleVerseModel verse)
    {
      return DataSource.SaveVerse(verse);
    }
    public void DeleteVerse(int id)
    {
      DataSource.DeleteVerse(id);
    }
  }
}
