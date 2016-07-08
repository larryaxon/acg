using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ACG.Common
{
  public class UserOption
  {
    private const string jsonValuePair = "\"{0}\": \"{1}\",";
    public string User { get; set; }
    public string OptionType { get; set; }
    public string OptionName { get; set; }
    public string Option { get; set; }
    public string Description { get; set; }
    public string LastModifiedBy { get; set; }
    public DateTime LastModifiedDateTime { get; set; }
    public Dictionary<string, object> Parms = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);

    public UserOption() { }
    public UserOption(string optiontext)
    {
      if (!string.IsNullOrEmpty(optiontext))
      {
        string inner = "\"" + CommonFunctions.getJsonValue(optiontext) + "\"";
        OptionType = CommonFunctions.getJsonName(optiontext);
        string[] parts = CommonFunctions.parseString(inner, new string[] { "," });
        if (parts != null && parts.GetLength(0) > 0)
        {
          foreach (string option in parts)
          {
            string[] optionparts = CommonFunctions.parseString(option, new string[] { ":" });
            if (optionparts != null && optionparts.GetLength(0) == 2)
            {
              string name = CommonFunctions.stripDelims(optionparts[0], '"').ToLower();
              string val = CommonFunctions.stripDelims(optionparts[1], '"');
              switch (name)
              {
                case "user": { User = val; break; }
                case "optionname": { OptionName = val; break; }
                case "option": { Option = val; loadParmsFromOptions();  break; }
                case "description": { Description = val; break; }
              }
            }
          }
        }
      }
    }
    private string createOptionFromParms()
    {
      StringBuilder sb = new StringBuilder();
      foreach (KeyValuePair<string, object> entry in Parms)
        sb.Append(string.Format("{0}:{1},", entry.Key, CommonFunctions.CString(entry.Value)));
      if (sb.Length > 0)
        sb.Length--; // strip last comma
      return sb.ToString();
    }
    private void loadParmsFromOptions()
    {
      if (!string.IsNullOrEmpty(Option))
      {
        string[] parms = CommonFunctions.parseString(Option);
        Parms.Clear();
        if (parms != null && parms.GetLength(0) > 0)
        {
          foreach (string parm in parms)
          {
            string[] parmparts = parm.Split(new char[] { ':' });
            if (parmparts != null && parmparts.GetLength(0) == 2)
              Parms.Add(parmparts[0], parmparts[1]);
          }
        }
      }
    }
    public new string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append(string.Format("\"{0}\": {{",OptionType));
      sb.Append(string.Format(jsonValuePair, "user", User));
      sb.Append(string.Format(jsonValuePair, "optiontype", OptionType));
      sb.Append(string.Format(jsonValuePair, "optionname", OptionName));
      sb.Append(string.Format(jsonValuePair, "description", Description));
      sb.Append(string.Format(jsonValuePair, "option", createOptionFromParms()));
      sb.Length--; // strip last comma
      sb.Append("}");
      return sb.ToString();
    }
    
  }
}
