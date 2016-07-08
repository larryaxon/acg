using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

using CCI.Common;

namespace CCI.Sys.Data
{
  public partial class DataSource
  {
    public CCITable getScreenDefinition(string screen, string section, string itemID)
    {
      string sectionwhere = string.Empty;
      string screenwhere = string.Empty;
      if (!string.IsNullOrEmpty(screen))
        screenwhere = string.Format("and s.Screen = '{0}'", screen);
      if (!string.IsNullOrEmpty(section))
        sectionwhere = string.Format("and s.ScreenSection = '{0}'", section);
      string sql = 
        @"select s.Screen, sec.IntVal1 SectionSequence, s.ScreenSection Section, s.Sequence ItemSequence, s.ItemID, m.Name, s.IsRecommended, s.ImagePath, description from ScreenDefinition s
          inner join MasterProductList m on s.ItemID = m.ItemID
          inner join ProductList p on p.ItemID = s.ItemID and p.Carrier = 'saddleback'
          inner join CodeMaster sec on sec.CodeValue =  s.ScreenSection
          where GETDATE() between s.StartDate and ISNULL(s.EndDate, '{1}') and GETDATE() between ISNULL(p.StartDate, '{0}') and ISNULL(p.EndDate, '{1}') {2} {3}
          order by Screen, sec.IntVal1, s.Sequence, s.ItemID";
      sql = string.Format(sql, CommonData.PastDateTime.ToShortDateString(), CommonData.FutureDateTime.ToShortDateString(), screenwhere, sectionwhere);
      DataSet ds = getDataFromSQL(sql);
      return CommonFunctions.convertDataSetToCCITable(ds);
    }
    public int? updateScreenDefinition(string screen, string section, string itemID, string startDate, string itemSequence, string isRecommended, string UseMRC, string imageFileName, string user)
    {
      string sql;
      if (!ExistsScreenDefinition(screen, section, itemID))
      {
        sql = string.Format(@"insert into ScreenDefinition (Screen, ScreenSection, ItemID, StartDate, Sequence, IsRecommended, UseMRC, ImagePath, EndDate, LastModifiedBy, LastModifiedDateTime) 
        Values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', null, '{8}', '{9}')",
          screen, section, itemID, CommonFunctions.CDateTime(startDate, CommonData.PastDateTime).ToShortDateString(), 
          CommonFunctions.CString(CommonFunctions.CInt(itemSequence)), CommonFunctions.CBoolean(isRecommended) ? "1" : "0",
          CommonFunctions.CBoolean(UseMRC) ? "1" : "0", imageFileName, user, DateTime.Now.ToString(CommonData.FORMATLONGDATETIME));
      }
      else
      {
        if (string.IsNullOrEmpty(screen) || string.IsNullOrEmpty(section) || string.IsNullOrEmpty(itemID))
          return -1;
        StringBuilder sb = new StringBuilder();
        bool hasUpdate = false;
        sb.Append("update ScreenDefinition Set ");
        if (!string.IsNullOrEmpty(itemSequence))
        {
          if (hasUpdate)
            sb.Append(", ");
          else
            hasUpdate = true;
          sb.Append(string.Format(" Sequence = {0}", CommonFunctions.CString(CommonFunctions.CInt(itemSequence))));
        }
        if (!string.IsNullOrEmpty(startDate))
        {
          if (hasUpdate)
            sb.Append(", ");
          else
            hasUpdate = true;
          sb.Append(string.Format(" StartDate = '{0}'", CommonFunctions.CString(CommonFunctions.CDateTime(startDate))));
        }
        if (!string.IsNullOrEmpty(isRecommended))
        {
            if (hasUpdate)
              sb.Append(", ");
            else
              hasUpdate = true;

            sb.Append(string.Format(" IsRecommended = {0}", CommonFunctions.CBoolean(isRecommended) ? "0" : "1"));
        }
        if (!string.IsNullOrEmpty(imageFileName))
        {
          if (hasUpdate)
            sb.Append(", ");
          else
            hasUpdate = true;
          sb.Append(string.Format(" ImagePath = '{0}'", CommonFunctions.CString(imageFileName)));
        }
        if (!string.IsNullOrEmpty(UseMRC))
        {
          if (hasUpdate)
            sb.Append(", ");
          else
            hasUpdate = true;
          sb.Append(string.Format(" UseMRC = '{0}'", CommonFunctions.CString(UseMRC)));
        }
        sb.Append(string.Format(" Where Screen = '{0}' and ScreenSection = '{1}' and ItemID = '{2}'", screen, section, itemID));
        sql = sb.ToString();

      }
      return updateDataFromSQL(sql);
    }
    public int? deleteScreenDefinition(string screen, string section, string itemID)
    {
      if (ExistsScreenDefinition(screen, section, itemID))
      {
        string sql = string.Format("delete from ScreenDefinition where Screen = '{0}' and ScreenSection = '{1}' and ItemID = '{2}'",screen, section, itemID);
        return updateDataFromSQL(sql);
      }
      return null;
    }
    public bool ExistsScreenDefinition(string screen, string section, string itemID)
    {
      string sql = string.Format("Select * from ScreenDefinition where Screen = '{0}' and ScreenSection = '{1}' and ItemID = '{2}'", screen, section, itemID);
      bool ret = false;
      DataSet ds = getDataFromSQL(sql);
      if (ds == null)
        return ret;
      if (ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
        ret = true;
      ds.Clear();
      ds = null;
      return ret;
    }
  }
}
