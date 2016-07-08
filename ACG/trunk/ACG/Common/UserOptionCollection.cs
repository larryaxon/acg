using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ACG.Common
{
  public class UserOptionCollection : IEnumerable<KeyValuePair<string, UserOption>>
  {
    private Dictionary<string, UserOption> _options = new Dictionary<string, UserOption>(StringComparer.CurrentCultureIgnoreCase);
    public UserOption this[string key]
    {
      get
      {
        if (_options.ContainsKey(key))
          return _options[key];
        else
          return null;
      }
      set
      {
        if (_options.ContainsKey(key))
          _options[key] = value;
        else
          _options.Add(key, value);
      }
    }

    public UserOptionCollection() { }
    public UserOptionCollection(string optiontext)
    {
      loadOptionText(optiontext);
    }
    public void Remove(string key)
    {
      if (_options.ContainsKey(key))
        _options.Remove(key);
    }
    public void Add(string key, UserOption option)
    {
      this[key] = option;
    }
    public void Add(UserOption option)
    {
      this[option.OptionName] = option;
    }
    public new string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("\"options\": {");
      foreach (KeyValuePair<string, UserOption> entry in _options)
      {
        sb.Append(entry.Value.ToString());
        sb.Append(",");
      }
      if (_options.Count > 0)
        sb.Length--; //strip last comma
      sb.Append("}");
      return sb.ToString();
    }
    private void loadOptionText(string optiontext)
    {
      if (!string.IsNullOrEmpty(optiontext))
      {
        string inner = CommonFunctions.stripDelims(optiontext, CommonData.cLEFTCURLY);
        string[] parts = CommonFunctions.parseString(inner);
        if (parts != null && parts.GetLength(0) > 0)
          foreach (string part in parts)
            Add(new UserOption(part));
      }
    }
    public UserOption createUserOption(string user, string type, string name)
    {
      UserOption option = new UserOption();
      option.User = user;
      option.OptionType = string.Format("NamedSearch:{0}",name);
      option.OptionName = type;
      option.Description = "Named Option";
      option.LastModifiedBy = user;
      option.LastModifiedDateTime = DateTime.Now;
      option.Option = ToString();
      return option;
    }
    public IEnumerator<KeyValuePair<string, UserOption>> GetEnumerator()
    {
      return _options.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
