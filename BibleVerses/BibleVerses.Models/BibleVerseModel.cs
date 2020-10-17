using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleVerses.Models
{
  public class BibleVerseModel
  {
    public int ID { get; set; }
    public string Username { get; set; }
    public Nullable<int> Sequence { get; set; }
    public string VerseGroup { get; set; }
    public string Book { get; set; }
    public Nullable<int> Chapter { get; set; }
    public Nullable<int> Verse { get; set; }
    public Nullable<int> ToVerse { get; set; }
    public string Text { get; set; }
    public Nullable<System.DateTime> StartDate { get; set; }
    public Nullable<System.DateTime> EndDate { get; set; }
    public string Comment { get; set; }
    public bool Active { get; set; }
    public Nullable<System.DateTime> DateTimeModified { get; set; }
    public string Language { get; set; }
    public string Translation { get; set; }
  }
}
