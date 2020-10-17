using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BibleVerses.Models
{
  public class BibleVerseGUIModel
  {
    public BibleVerseModel model { get; set;  }
    public AuthenticationTokenModel AuthenticationToken { get; set; }
    public string Verse { get; set; }
  }
}