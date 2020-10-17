using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using BibleVerses.Models;

using BibleVerseDAO;
using System.Net.Http.Headers;
using System.Data.Entity.Core.Metadata.Edm;

namespace BibleVerses.Manager
{
  public static class DataSource
  {
    public static string testDBConnection()
    {
      try
      {
        using (ACGEntities ef = new ACGEntities())
        {
          var rec = ef.Logins.FirstOrDefault();
          return "Success";
        }
      }
      catch (Exception ex)
      {
        return "DB Connection Error: " + ex.Message + ", Stack trace = " + ex.StackTrace;
      }

    }
    public static IEnumerable<CodeMasterModel> getCodes()
    {
      using (ACGEntities ef = new ACGEntities())
      {
        return ef.CodeMasters.Where(cm => cm.CodeType.Equals("Codes", StringComparison.InvariantCultureIgnoreCase)).Select(cm => new CodeMasterModel()
        {
          CodeType = cm.CodeType,
          Code = cm.Code,
          Description = cm.Description
        });

      }
    }
    public static LoginModel GetLogin(string username)
    {
      using (ACGEntities ef = new ACGEntities())
      {
        return ef.Logins.Where(l => l.Username.Equals(username, StringComparison.CurrentCultureIgnoreCase))
          .Select(log => new LoginModel()
          {
            UserName = log.Username, 
            Password = log.Password,
            Role = log.Role,
            IsAuthenticated = true
          })
          .FirstOrDefault();
      }
    }
    public static IEnumerable<BibleVerseModel> GetBibleVerses(string username, string group)
    {
      using (ACGEntities ef = new ACGEntities())
      {
        return ef.BibleVerses
          .Where(v => v.Username.Equals(username, StringComparison.CurrentCultureIgnoreCase) &&
          v.VerseGroup.Equals(group, StringComparison.CurrentCultureIgnoreCase) &&
          (v.StartDate == null || v.StartDate <= DateTime.Today) &&
          (v.EndDate == null || v.EndDate >= DateTime.Today))
          .OrderBy(v => v.Sequence)
          .ToList()
          .Select(v => ToBibleVerseModel(v));
      }
    }
    public static BibleVerseModel GetOneVerse(int id)
    {
      using (ACGEntities ef = new ACGEntities())
      {
        return ef.BibleVerses.Where(v => v.ID == id)
          .Select(v => ToBibleVerseModel(v))
          .ToList()
          .FirstOrDefault();
      }
    }
    public static int GetLastVerseDisplayed(string username, string group)
    {
      using (ACGEntities ef = new ACGEntities())
      {
        IEnumerable<LastVerseDisplayed> lastVerses = ef.LastVerseDisplayeds.Where(v => v.Username.Equals(username, StringComparison.CurrentCultureIgnoreCase) &&
                                                        v.VerseGroup.Equals(group, StringComparison.CurrentCultureIgnoreCase));
        if (lastVerses == null || lastVerses.Count() == 0)
          return -1;
        return (int)lastVerses.First().LastVerseID;

             
      }
    }
    public static void updateLastVerseDisplayed(string username, string group, int lastVerse)
    {
      using (ACGEntities ef = new ACGEntities())
      {
        IEnumerable<LastVerseDisplayed> lastverses = ef.LastVerseDisplayeds.Where(lv => lv.Username.Equals(username, StringComparison.CurrentCultureIgnoreCase) &&
                                                                     lv.VerseGroup.Equals(group, StringComparison.CurrentCultureIgnoreCase));
        if (lastverses == null || lastverses.Count() == 0)
        {
          ef.LastVerseDisplayeds.Add(new LastVerseDisplayed()
          {
            Username = username,
            VerseGroup = group,
            LastVerseID = lastVerse
          });
        }
        else
        {
          lastverses.First().LastVerseID = lastVerse;
        }
        ef.SaveChanges();
      }
    }
    public static int SaveVerse(BibleVerseModel model)
    {
      using (ACGEntities ef = new ACGEntities())
      {
        BibleVerse verse = new BibleVerse()
        {
          Username = model.Username,
          VerseGroup = model.VerseGroup,
          Sequence = model.Sequence,
          Book = model.Book,
          Chapter = model.Chapter,
          ToVerse = model.ToVerse,
          Text = model.Text,
          StartDate = model.StartDate,
          EndDate = model.EndDate,
          Comment = model.Comment,
          Active = model.Active,
          DateTimeModified = DateTime.Now,
          Language = model.Language,
          Translation = model.Translation
        };
        ef.BibleVerses.Add(verse);
        ef.SaveChanges();
        return verse.ID;        
      }
    }
    public static void DeleteVerse(int id)
    {
      using (ACGEntities ef = new ACGEntities())
      {
        IEnumerable<BibleVerse> verses = ef.BibleVerses.Where(v => v.ID == id);
        if (verses != null && verses.Count() > 0)
          ef.BibleVerses.Remove(verses.First());
      }
    }
    private static BibleVerseModel ToBibleVerseModel (BibleVerse v)
    {
      return new BibleVerseModel()
      {
        ID = v.ID,
        Username = v.Username,
        Sequence = v.Sequence,
        VerseGroup = v.VerseGroup,
        Book = v.Book,
        Chapter = v.Chapter,
        Verse = v.Verse,
        ToVerse = v.ToVerse,
        Text = v.Text,
        StartDate = v.StartDate,
        EndDate = v.EndDate,
        Comment = v.Comment,
        Active = v.Active,
        DateTimeModified = v.DateTimeModified,
        Language = v.Language,
        Translation = v.Translation
      };
    }
  }
}
