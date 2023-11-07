using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CCI.Common
{
  public static class ServerResponseSerializer
  {
    private static string[] commadelim = new string[] { "," };
    private static string[] colondelim = new string[] { ",", ":" };
    public static bool IsNewTableFormat = true;
    public static ServerResponse FromJson(string json)
    {
      bool newTableFormat = IsNewTableFormat;
      int[] processOrder = new int[] { 2, 0, 1, 3 }; // order in which we process the sections... so we can process the options first
      ServerResponse res = new ServerResponse();
      if (!json.StartsWith("{ \"results\": { "))
        throw new Exception("Not a valid ATKServerResponse json string");
      json = CommonFunctions.stripDelims(json, CommonData.cLEFTCURLY); // strip off container braces
      string[] parts = CommonFunctions.parseString(json, commadelim);
      if (parts != null)
      {
        for (int i = 0; i < parts.GetLength(0); i++)
        {
          int iPart = i;
          if (i < processOrder.GetLength(0))
            iPart = processOrder[i];
          string name = CommonFunctions.getJsonName(parts[iPart]); //  
          string list;
          string[] listParts;
          string subList;
          string[] subListParts;
          switch (name.ToLower())
          {
            case "results":
              list = CommonFunctions.stripDelims(parts[iPart], CommonData.cLEFTCURLY);
              listParts = CommonFunctions.parseString(list, commadelim);
              if (listParts != null)
              {
                for (int j = 0; j < listParts.GetLength(0); j++)
                {
                  string resultName = CommonFunctions.getJsonName(listParts[j]).Trim().ToLower();
                  switch (resultName)
                  {
                    case "form":
                      res.Results.Add(JsonToForm(listParts[j], newTableFormat));
                      break;
                    case "parameters":  // hash tables are always serialized with the tag "parameters"
                      res.Results.Add(JsonToHashtable(listParts[j]));
                      break;
                    case "cciformitem":
                      res.Results.Add(JsonToFormItem(listParts[j]));
                      break;
                    case "table":
                      res.Results.Add(JsonToTable(listParts[j], newTableFormat));
                      break;
                    case "":
                      res.Results.Add(listParts[j]);
                      break;
                  }
                }
              }
              break;
            case "securitycontext":
              res.SecurityContext = new SecurityContext(parts[iPart]);
              break;
            case "options":
              // this is in the form options: { parameters: {}} so we have to get the inner list
              list = CommonFunctions.stripDelims(parts[iPart], CommonData.cLEFTCURLY);
              list = CommonFunctions.stripDelims(list, CommonData.cLEFTCURLY); // get the inner list
              listParts = CommonFunctions.parseString(list, commadelim);

              if (listParts != null)
              {
                for (int j = 0; j < listParts.GetLength(0); j++)
                {
                  subList = CommonFunctions.stripDelims(listParts[j], CommonData.cLEFTCURLY);
                  subListParts = CommonFunctions.parseString(subList, colondelim);
                  string key = CommonFunctions.stripDelims(subListParts[0], '"').ToLower();
                  string val = CommonFunctions.stripDelims(subListParts[1], '"');
                  res.Options.Add(key, val);
                }
              }
              if (res.Options.ContainsKey(CommonData.NEWTABLEFORMAT))
                newTableFormat = CommonFunctions.CBoolean(res.Options[CommonData.NEWTABLEFORMAT]);
              break;
            case "errors":
              list = CommonFunctions.stripDelims(parts[iPart], CommonData.cLEFTSQUARE);
              if (!string.IsNullOrEmpty(list))
              {
                listParts = CommonFunctions.parseString(parts[iPart], commadelim);
                if (listParts != null)
                {
                  for (int j = 0; j < listParts.GetLength(0); j++)
                    res.Errors.Add(CommonFunctions.stripDelims(listParts[j], '"'));
                }
              }
              break;
          }
        }
      }
      return res;
    }
    #region public ToJson overloads
    public static string ToJson(SecurityContext o)
    {
      return toJson(o, new StringBuilder()).ToString();
    }
    public static string ToJson(ServerResponse o)
    {
      return toJson(o, new StringBuilder()).ToString();
    }
    public static string ToJson(CCITable o)
    {
      return toJson(o, true ,new StringBuilder()).ToString();
    }
    public static string ToJson(CCITable o, bool newTableFormat)
    {
      return toJson(o, newTableFormat, new StringBuilder()).ToString();
    }
    public static string ToJson(CCIForm o)
    {
      return toJson(o, new StringBuilder()).ToString();
    }
    public static string ToJson(CCIFormItem o)
    {
      return toJson(o, new StringBuilder()).ToString();
    }
    public static string ToJson(Hashtable o)
    {
      return toJson(o, new StringBuilder()).ToString();
    }
    public static string ToJson(ArrayList o)
    {
      return toJson(o, new StringBuilder()).ToString();
    }
    public static string ToJson(PickListEntries o)
    {
      return toJson(o, new StringBuilder()).ToString();
    }
    #endregion

    #region public JsonTo<Object> methods

    public static CCITable JsonToTable(string json, bool isNewFormat)
    {
      CCITable table = new CCITable();
      if (isNewFormat)
      {
        string rowList = CommonFunctions.stripDelims(json, CommonData.cLEFTSQUARE); // strip off outer array delins [ ]
        string[] rowListParts = CommonFunctions.parseString(rowList, commadelim);
        Dictionary<int, Dictionary<int, PickListEntries>> picklists = new Dictionary<int, Dictionary<int, PickListEntries>>();
        for (int iRow = 0; iRow < rowListParts.GetLength(0); iRow++)
        {
          string row = CommonFunctions.stripDelims(rowListParts[iRow], CommonData.cLEFTSQUARE);
          string[] rowParts = CommonFunctions.parseString(row, commadelim);
          object[] cells = new object[rowParts.GetLength(0)];
          // parse cell
          for (int iCol = 0; iCol < rowParts.GetLength(0); iCol++)
          {
            string[] cellParts = CommonFunctions.parseString(CommonFunctions.stripDelims(rowParts[iCol], CommonData.cLEFTCURLY), commadelim);
            if (cellParts != null) // name, value, type
            {
              string name = string.Empty;
              string val = string.Empty;
              string type = "string";
              for (int iPart = 0; iPart < cellParts.GetLength(0); iPart++)
              {
                string picklist = string.Empty;
                string nameName = CommonFunctions.getJsonName(cellParts[iPart]).ToLower();
                string nameValue = CommonFunctions.getJsonValue(cellParts[iPart]);
                switch (nameName)
                {
                  case "fieldname":
                    if (!table.ContainsColumn(nameValue))
                      table.AddColumn(nameValue);
                    val = nameValue;
                    break;
                  case "type":
                    type = nameValue;
                    break;
                  case "value":
                    val = nameValue;
                    break;
                  case "picklist":
                    picklist = "{{\"" + nameValue + "\"}}";
                    PickListEntries p = JsonToPickList(picklist);
                    Dictionary<int, PickListEntries> newList;
                    if (picklists.ContainsKey(iRow))
                      newList = picklists[iRow];
                    else
                    {
                      newList = new Dictionary<int, PickListEntries>();
                      picklists.Add(iRow, newList);
                    }
                    if (newList.ContainsKey(iCol))
                      newList[iCol] = p;
                    else
                      newList.Add(iCol, p);
                    break;
                }
                cells[iCol] = CommonFunctions.toValue(val, type); // convert to the correct data type
              }
            }
          }
          table.AddRow(cells);
        }
        foreach (KeyValuePair<int, Dictionary<int, PickListEntries>> plrow in picklists)
        {
          foreach (KeyValuePair<int, PickListEntries> plcell in plrow.Value)
            table.setPickList(plrow.Key, table.Column(plcell.Key), plcell.Value);
        }
      }
      else
      {
        string subList = CommonFunctions.stripDelims(json, CommonData.cLEFTCURLY);
        string[] subListParts = CommonFunctions.parseString(subList, commadelim);
        string columnsName = CommonFunctions.getJsonName(subListParts[0]);
        string columnName = string.Empty;
        if (columnsName.Equals("columns", StringComparison.CurrentCultureIgnoreCase))
        {
          string columnList = CommonFunctions.stripDelims(subListParts[0], CommonData.cLEFTSQUARE);
          string[] columnNames = CommonFunctions.parseString(columnList, commadelim);
          for (int k = 0; k < columnNames.GetLength(0); k++)
          {
            columnName = columnNames[k];
            columnName = CommonFunctions.stripDelims(columnName, CommonData.cQUOTE[0]);
            columnName = CommonFunctions.stripDelims(columnName, CommonData.cDOUBLEQUOTE);
            table.AddColumn(columnName);
          }
        }
        string rowsName = CommonFunctions.getJsonName(subListParts[1]);
        string cellValue = string.Empty;
        if (rowsName.Equals("rows", StringComparison.CurrentCultureIgnoreCase))
        {
          string rowsList = CommonFunctions.stripDelims(subListParts[1], CommonData.cLEFTSQUARE);
          string[] rows = CommonFunctions.parseString(rowsList, commadelim);
          for (int k = 0; k < rows.GetLength(0); k++)
          {
            string row = CommonFunctions.stripDelims(rows[k], CommonData.cLEFTSQUARE);
            string[] cells = CommonFunctions.parseString(row, commadelim);
            if (cells.GetLength(0) != table.NumberColumns)
              throw new Exception("Cells count do not match columns");
            for (int k1 = 0; k1 < cells.GetLength(0); k1++)
            {
              cellValue = cells[k1];
              cellValue = CommonFunctions.stripDelims(cellValue, CommonData.cQUOTE[0]); // strip off quotes from each value 
              cellValue = CommonFunctions.stripDelims(cellValue, CommonData.cDOUBLEQUOTE); // strip off the doublequotes from each value 
              cells[k1] = cellValue;
            }
            table.AddRow(cells);
          }
        }
      }
      return table;
    }

    public static PickListEntries JsonToPickListEntries(string json)
    {
      PickListEntries list = new PickListEntries();
      string subList = CommonFunctions.stripDelims(json, CommonData.cLEFTCURLY);
      string[] subListParts = CommonFunctions.parseString(subList, commadelim);
      string rowsList = CommonFunctions.stripDelims(subListParts[1], CommonData.cLEFTSQUARE);
      string[] rows = CommonFunctions.parseString(rowsList, commadelim);
      for (int k = 0; k < rows.GetLength(0); k++)
      {
        string row = CommonFunctions.stripDelims(rows[k], CommonData.cLEFTSQUARE);
        string[] cells = CommonFunctions.parseString(row, commadelim);
        if (cells.GetLength(0) == 2)
        {
          PickListEntry entry = new PickListEntry();
          entry.ID = cells[0];
          entry.Description = cells[1];
          list.Add(entry);
        }
      }
      return list;
    }
    public static CCIForm JsonToForm(string json)
    {
      return JsonToForm(json, IsNewTableFormat);
    }
    public static CCIForm JsonToForm(string json, bool newTableFormat)
    {
      CCIForm form = new CCIForm();
      string subList = CommonFunctions.stripDelims(json, CommonData.cLEFTCURLY);
      // OK sublist has two parts: name, and value
      string[] subListParts = CommonFunctions.parseString(subList, commadelim);
      if (subListParts == null || subListParts.GetLength(0) != 2)
        throw new Exception("Invalid Form Json");
      form.Name = CommonFunctions.getJsonValue(subListParts[0]);
      string value = CommonFunctions.stripDelims(subListParts[1], CommonData.cLEFTCURLY);
      subListParts = CommonFunctions.parseString(value, commadelim);
      for (int k = 0; k < subListParts.GetLength(0); k++)
        form.Add(JsonToFormItem(subListParts[k],newTableFormat));
      return form;
    }
    public static CCIFormItem JsonToFormItem(string json)
    {
      return JsonToFormItem(json, IsNewTableFormat);
    }
    public static CCIFormItem JsonToFormItem(string json, bool newTableFormat)
    {
      CCIFormItem item = new CCIFormItem();
      item.ID = CommonFunctions.getJsonName(json);
      string subList = CommonFunctions.stripDelims(json, CommonData.cLEFTCURLY);
      string[] subListParts = CommonFunctions.parseString(subList, commadelim);
      for (int k = 0; k < subListParts.GetLength(0); k++)
      {
        string key = CommonFunctions.getJsonName(subListParts[k]).ToLower();
        string[] valParts = CommonFunctions.parseString(subListParts[k], colondelim);

        string val = string.Empty;
        if (!valParts[1].Trim().StartsWith(CommonData.cLEFTCURLY.ToString()))
          val = CommonFunctions.stripDelims(valParts[1], '"');
        else
          val = valParts[1];

        switch (key)
        {
          case "type":
            item.FormItemType = getType(val);
            break;
          case "value":
            switch (item.FormItemType)
            {
              case FormItemTypes.Bool:
                item.Value = CommonFunctions.CBoolean(val);
                break;
              case FormItemTypes.Date:
              case FormItemTypes.DateTime:
                item.Value = CommonFunctions.CDateTime(val);
                break;
              case FormItemTypes.String:
                item.Value = val;
                break;
              case FormItemTypes.PickList:
                string[] subValParts = CommonFunctions.parseString(CommonFunctions.stripDelims(val, CommonData.cLEFTCURLY), colondelim);
                item.Value = JsonToTable(subValParts[1], newTableFormat);
                break;
              case FormItemTypes.Table:
                item.Value = JsonToTable(val, newTableFormat);
                break;
            }
            break;
        }
      }

      return item;
    }
    public static PickListEntries JsonToPickList(string json)
    {
      PickListEntries parms = new PickListEntries();
      // parameters: {} so we have to get the inner list
      string subList = CommonFunctions.stripDelims(json, CommonData.cLEFTCURLY);
      string[] subListParts = CommonFunctions.parseString(subList, commadelim);
      for (int k = 0; k < subListParts.GetLength(0); k++)
      {
        string[] subListParts2 = CommonFunctions.parseString(CommonFunctions.stripDelims(subListParts[k], CommonData.cLEFTCURLY), commadelim);
        PickListEntry val = new PickListEntry();
        val.Description = CommonFunctions.getJsonValue(subListParts2[1]);
        val.ID =  CommonFunctions.getJsonValue(subListParts2[0]).ToLower();;
        parms.Add(val);
      }
      return parms;
    }
    public static Hashtable JsonToHashtable(string json)
    {
      Hashtable parms = new Hashtable();
      // parameters: {} so we have to get the inner list
      string subList = CommonFunctions.stripDelims(json, CommonData.cLEFTCURLY);
      string[] subListParts = CommonFunctions.parseString(subList, commadelim);
      for (int k = 0; k < subListParts.GetLength(0); k++)
      {
        string[] subListParts2 = CommonFunctions.parseString(subListParts[k], colondelim);
        string key = CommonFunctions.stripDelims(subListParts2[0], CommonData.cDOUBLEQUOTE).ToLower();
        string val = CommonFunctions.stripDelims(subListParts2[1], CommonData.cDOUBLEQUOTE);
        parms.Add(key, val);
      }
      return parms;
    }
    
    #endregion public JsonTo<Object> methods

    #region private toJson overloads
    private static StringBuilder toJson(SecurityContext s, StringBuilder sb)
    {
      sb.Append(s.ToJson());
      return sb;
    }
    private static StringBuilder toJson(ServerResponse res, StringBuilder sb)
    {
      bool isNewFormat = IsNewTableFormat;
      if (res.Options.ContainsKey(CommonData.NEWTABLEFORMAT))
        isNewFormat = CommonFunctions.CBoolean(res.Options[CommonData.NEWTABLEFORMAT]);
      sb.Append("{ \"results\": { ");
      foreach (object result in res.Results)
      {
        if (result != null)
        {
          Type t = result.GetType();
          switch (t.Name.ToLower())
          {
            case "cciform":
              sb = toJson((CCIForm)result, sb, isNewFormat);
              break;
            case "hashtable":
              sb = toJson((Hashtable)result, sb);
              break;
            case "ccitable":
              sb = toJson((CCITable)result, isNewFormat, sb);
              break;
            case "cciformitem":
              sb = toJson((CCIFormItem)result, sb, isNewFormat);
              break;
            case "string":
              sb.Append(string.Format("\"{0}\"",fixStringToJson(CommonFunctions.CString(result))));
              break;
          }
          sb.Append(",");
        }
      }
      if (sb[sb.Length - 1] == ',')
        sb.Length = sb.Length - 1;

      sb.Append("},");
      sb = toJson(res.SecurityContext, sb);
      sb.Append(", \"options\": { ");
      sb = toJson(res.Options, sb);
      sb.Append(" }, \"errors\": [ ");
      sb = toJson(res.Errors, sb);
      sb.Append(" ] }");
      return sb;
    }
    private static StringBuilder toJson(CCITable table, bool isNewFormat, StringBuilder sb)
    {
      sb.Append("\"table\": ");
      if (isNewFormat)
      {
        /*
    "table": [
        [
            {
                "fieldname": "Name",
                "value": "juan",
                "type": "string",
                "picklist": { "id1": "value1", "id2": "value2"... } // optional pick list
            },
            {
                "fieldname": "Name2",
                "value": "juan",
                "type": "string"
            }
        ],
        [
            {
                "fieldname": "Name",
                "value": "larry",
                "type": "string"
            },
            {
                "fieldname": "Name2",
                "value": "larry",
                "type": "string"
            }
        ]
    ]
         */
        sb.Append("[ ");
        for (int iRow = 0; iRow < table.NumberRows; iRow++)
        {
          sb.Append("[ ");
          for (int iCol = 0; iCol < table.NumberColumns; iCol++)
          {
            string column = table.Column(iCol);
            string type = table.getDataType(column);
            string strPickList = string.Empty;
            PickListEntries pickList = table.getPickList(iRow, column);
            if (pickList != null)
              strPickList = ", "+toJsonPicklist(pickList);
            if (table[iRow, iCol] != null)
              type = getJsonType(table[iRow, iCol].GetType().Name);
            sb.Append(string.Format("{{ \"fieldname\": \"{0}\", \"value\": \"{1}\", \"type\": \"{2}\"{3} }},",
              table.Column(iCol), fixStringToJson(CommonFunctions.CString(table[iRow, iCol])), type, strPickList));
          }
          if (table.NumberColumns > 0)
            sb.Length = sb.Length - 1; // strip off last comma
          sb.Append(" ],");
        }
        if (table.NumberRows > 0)
          sb.Length = sb.Length - 1; // strip off last comma
        sb.Append(" ]");
      }
      else
      {
        sb.Append("{ \"columns\": [ ");
        for (int i = 0; i < table.NumberColumns; i++)
          sb.Append(string.Format("\"{0}\",", table.Column(i)));
        sb.Length = sb.Length - 1; //strip last comma
        sb.Append(" ], \"rows\": [ ");
        for (int i = 0; i < table.NumberRows; i++)
        {
          sb.Append("[ ");
          for (int j = 0; j < table.NumberColumns; j++)
            sb.Append(string.Format("\"{0}\",", CommonFunctions.CString(table[i, j])));
          sb.Length = sb.Length - 1; //strip last comma
          sb.Append(" ],");
        }
        sb.Length = sb.Length - 1; //strip comma
        sb.Append(" ] }");
      }
      return sb;
    }
    private static StringBuilder toJson(PickListEntries table, StringBuilder sb)
    {
      // PickListEntry just has two columns ID, Description
      sb.Append("\"table\": { \"columns\": [ \"ID\", \"Description\" ], \"rows\": [ ");
      for (int i = 0; i < table.Count; i++)
      {
        sb.Append(string.Format("[ \"{0}\", \"{1}\" ]", CommonFunctions.CString(table[i].OriginalID, table[i].Description)));
      }
      sb.Length = sb.Length - 1; //strip comma
      sb.Append(" ] }");
      return sb;
    }
    private static StringBuilder toJson(CCIForm form, StringBuilder sb)
    {
      return toJson(form, sb, IsNewTableFormat);
    }
    private static StringBuilder toJson(CCIForm form, StringBuilder sb, bool isNewFormat)
    {
      sb.Append("\"form\": { \"name\": \"");
      sb.Append(form.Name);
      sb.Append("\", \"value\": { ");
      for (int i = 0; i < form.Count; i++)
      {
        sb = toJson((CCIFormItem)form[i], sb, isNewFormat); // type ATKFormItem
        sb.Append(",");
      }
      if (form.Count > 0)
        sb.Length -= 1; // strip last comma

      sb.Append(" } }");
      return sb;
    }
    private static StringBuilder toJson(CCIFormItem item, StringBuilder sb)
    {
      return toJson(item, sb, IsNewTableFormat);
    }
    private static StringBuilder toJson(CCIFormItem item, StringBuilder sb, bool isNewFormat)
    {
      sb.Append("\"");
      sb.Append(item.ID);
      sb.Append("\": { ");
      sb.Append("\"type\": \"");
      sb.Append(item.FormItemType.ToString());
      sb.Append("\", \"value\": ");
      switch (item.FormItemType)
      {
        case FormItemTypes.Bool:
          sb.Append(CommonFunctions.CBoolean(item.Value).ToString().ToLower());
          break;
        case FormItemTypes.Date:
          sb.Append("\"");
          sb.Append(CommonFunctions.CDateTime(item.Value).ToShortDateString());
          sb.Append("\"");
          break;
        case FormItemTypes.String:
        case FormItemTypes.DateTime:
          sb.Append("\"");
          sb.Append(fixStringToJson(CommonFunctions.CString(item.Value)));
          sb.Append("\"");
          break;
        case FormItemTypes.PickList:
        case FormItemTypes.Table:
          sb.Append("{ ");
          sb = toJson((CCITable)item.Value, isNewFormat, sb);
          sb.Append(" }");
          break;
      }
      sb.Append(" }");
      return sb;
    }
    private static StringBuilder toJson(Hashtable parms, StringBuilder sb)
    {
      sb.Append("\"parameters\": { ");
      foreach (DictionaryEntry parm in parms)
      {
        sb.Append(string.Format("\"{0}\": \"{1}\",",
          CommonFunctions.CString(parm.Key).ToLower(),
          CommonFunctions.CString(parm.Value).ToLower()));
      }
      if (parms.Count > 0)
        sb.Length = sb.Length - 1;  // strip off last comma
      sb.Append("}");
      return sb;
    }
    private static StringBuilder toJson(ArrayList array, StringBuilder sb)
    {
      foreach (object o in array)
        sb.Append(string.Format("\"{0}\",", CommonFunctions.CString(o)));
      if (array.Count > 0)
        sb.Length -= 1;
      return sb;
    }
    #endregion
    #region private utility methods
    private static FormItemTypes getType(string command)
    {
      foreach (var cmd in Enum.GetValues(typeof(FormItemTypes)))
      {
        if (command.Equals(cmd.ToString(), StringComparison.CurrentCultureIgnoreCase))
          return (FormItemTypes)cmd;
      }
      return FormItemTypes.String;
    }
    private static string getJsonType(string typeIn)
    {
      switch (typeIn.ToLower())
      {
        case "string":
        case "int":
        case "datetime":
        case "date":
        case "decimal":
          return typeIn.ToLower();
        case "int32":
        case "byte":
        case "long":
          return "int";
        default:
          return "string";
      }
    }
    private static string toJsonPicklist(PickListEntries t)
    {
      StringBuilder sb = new StringBuilder();
      sb.Append(" \"picklist\": [ ");
      SortedDictionary<string, PickListEntry> list = t.SortedBy("Description");
      foreach (KeyValuePair<string, PickListEntry> entry in list)
      {
        sb.Append(string.Format("{{ \"code\": \"{0}\", \"description\": \"{1}\" }},", entry.Value.ID, CommonFunctions.CString(entry.Value.Description)));
      }
      if (t.Count > 0)
        sb.Length = sb.Length - 1; //strip off the last comma
      sb.Append(" ]");
      return sb.ToString();
    }

    private static string fixStringToJson(string valueToFix)
    {
      string valueFixed = string.Empty;

      for (int i = 0; i < valueToFix.Length; i++)
      {
        char curChar = valueToFix[i];
        switch (curChar)
        {
          case '\\':
            valueFixed += "\\\\";
            break;
          case '"':
            valueFixed += "\\\"";
            break;
          case '\b':
            valueFixed += "\\b";
            break;
          case '\t':
            valueFixed += "\\t";
            break;
          case '\n':
            valueFixed += "\\n";
            break;
          case '\f':
            valueFixed += "\\f";
            break;
          case '\r':
            valueFixed += "\\r";
            break;
          default:
            valueFixed += curChar;
            break;
        }
      }
      //valueFixed = valueToFix.Replace("\\", "\\\\");
      ////valueFixed = valueToFix.Replace('/', "\\/");
      //valueFixed = valueToFix.Replace("\"", "\\\"");
      //valueFixed = valueToFix.Replace('\b', '\b');
      //valueFixed = valueToFix.Replace("\t", "\\t");
      //valueFixed = valueToFix.Replace("\n", "\\n");
      //valueFixed = valueToFix.Replace("\f", "\\f");
      //valueFixed = valueToFix.Replace("\r", "\\r");
      ////valueFixed = valueToFix.Replace("\u", "\\u");
      return valueFixed;
    }

    private static string fixStringFromJson(string valueToFix)
    {
      string valueFixed = valueToFix;
      valueFixed = valueFixed.Replace("\\b", "\b");
      valueFixed = valueFixed.Replace("\\t", "\t");
      valueFixed = valueFixed.Replace("\\n", "\n");
      valueFixed = valueFixed.Replace("\\f", "\f");
      valueFixed = valueFixed.Replace("\\r", "\r");
      valueFixed = valueFixed.Replace("\\\"", "\"");
      valueFixed = valueFixed.Replace("\\", "");
      //valueFixed = valueFixed.Replace("\\\\", "\\");
      //valueFixed = valueToFix.Replace('/', "\\/");
      //valueFixed = valueToFix.Replace("\u", "\\u");
      return valueFixed;
    }

   #endregion
  }
}
