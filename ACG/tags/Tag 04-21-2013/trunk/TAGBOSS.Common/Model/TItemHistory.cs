using System;

namespace TAGBOSS.Common.Model
{
  public class TItemHistory
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
  }
}
