using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BibleVerses.Models;

namespace BibleVerses.Common
{
  public static class CommonFunctions
  {
    public static string MakeFormattedVerse(BibleVerseModel model)
    {
      return model.Book + " "
        + (string.IsNullOrWhiteSpace(model.Translation) ? string.Empty : " (" + model.Translation + ") ")
        + model.Chapter + ":"
        + model.Verse.ToString()
        + (model.ToVerse == null ? string.Empty : "-" + ((int)model.ToVerse).ToString())
        + " " + model.Text;
    }
  }
}
