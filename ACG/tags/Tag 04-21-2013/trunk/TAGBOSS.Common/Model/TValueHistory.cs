using System;

namespace TAGBOSS.Common.Model
{
  public class TValueHistory
  {
    /// <summary>
    /// In this case, it is the StartDate, converted to a string.
    /// While we support both get and set for compatibility, it is recommended for clarity 
    /// that the set for startDate be used instead of set for this property to set the value;
    /// </summary>
    public string Id
    {
      get { return StartDate.ToString("yyyyMMdd"); }    //Normalize id so it can be sorted and will yield proper date order
      set
      {
        //convert to short date and back so we drop any time portion
        StartDate = Convert.ToDateTime(Convert.ToDateTime(value).ToShortDateString());
      }
    }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Value { get; set; }
    public string ValueType { get; set; }
    public string LastModifiedBy { get; set; }
    public DateTime LastModifiedDateTime { get; set; }

    /// <summary>
    /// This is where we keep where the value came from (like func or ref or refinherit)
    /// </summary>
    public string ReferenceValueSource { get; set; }

    public string ToString() 
    {
      string toString = "";

      toString = "StartDate: " + (Id == null ? "(empty)" : Id);
      toString += "; ValueType: " + (ValueType == null ? "(empty)" : ValueType);
      toString += "; Value: " + (Value == null ? "(empty)" : Value);

      return toString;
    }

    public object Clone() 
    {
      TValueHistory valueHistoryNew = new TValueHistory();
      valueHistoryNew.StartDate = this.StartDate;
      valueHistoryNew.EndDate = this.EndDate;
      valueHistoryNew.ValueType = this.ValueType;
      valueHistoryNew.Value = this.Value;
      valueHistoryNew.LastModifiedBy = this.LastModifiedBy;
      valueHistoryNew.LastModifiedDateTime = this.LastModifiedDateTime;
      valueHistoryNew.ReferenceValueSource = this.ReferenceValueSource;

      return valueHistoryNew;
    }
  }
}
