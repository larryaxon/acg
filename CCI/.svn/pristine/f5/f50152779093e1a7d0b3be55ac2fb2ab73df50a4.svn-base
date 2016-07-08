using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TAGBOSS.Common;

namespace TAGBOSS.Common.Model
{
  /// <summary>
  /// The base class for date history elements node of Item or TAGAttribute objects
  /// </summary>
  [SerializableAttribute]
  public class DateHistory
  {
    // TODO: need to add old startdate, old end date, and maybe dirty for each, also need null values?
    private DateTime startDate;
    private DateTime endDate;
    private DateTime oldStartDate;
    private DateTime oldEndDate;
    private bool isDirty = false;
    private bool isDeleted = false;
    private bool isReadOnly = false;
    private string lastModifiedBy;
    private DateTime lastModifiedDateTime;
    //private Dictionaries dictionary = null;

    public Dictionaries Dictionary
    {
      get { return DictionaryFactory.getInstance().getDictionary(); }
      //set { dictionary = value; }
    }
    public DateTime OldStartDate
    {
      get { return oldStartDate; }
      //set { oldStartDate = value; }
    }

    public DateTime OldEndDate
    {
      get { return oldEndDate; }
      //set { oldEndDate = value; }
    }

    public DateHistory()
    {
      startDate = TAGFunctions.PastDateTime;
      endDate = TAGFunctions.FutureDateTime;
    }
    /// <summary>
    /// Has this History instance been flagged as deleted?
    /// </summary>
    public bool Deleted
    {
      get { return isDeleted; }
      set { isDeleted = value; }
    }
    /// <summary>
    /// Has this instance been flagged as Dirty
    /// </summary>
    public bool Dirty
    {
      get { return isDirty; }
      set { isDirty = value; }
    }
    /// <summary>
    /// Is this date history record read only?
    /// </summary>
    public bool ReadOnly
    {
      get { return isReadOnly; }
      set { isReadOnly = value; }
    }
    /// <summary>
    /// In order for this History to be used as an IDataClassItem, we need to expose a unique string ID. 
    /// In this case, it is the StartDate, converted to a string.
    /// While we support both get and set for compatibility, it is recommended for clarity that
    /// the set for startDate be used instead of set for this property to set the value;
    /// </summary>
    public string ID
    {
      /*
       * In order to be used as an element in DataClass, we need to expose a unique string ID. In this case, it is the StartDate,
       * converted to a string.
       */
      get { return startDate.ToString("yyyyMMdd"); }    // LLA 9/2/2009 normalize id so it can be sorted and will yield proper date order
      set
      {
        startDate = Convert.ToDateTime(Convert.ToDateTime(value).ToShortDateString());  //convert to short date and back so we drop any time portion
        isDirty = true;
      }
    }
    /// <summary>
    /// The date on which this element becomes effective
    /// </summary>
    public DateTime StartDate
    {
      get { return startDate; }
      set
      {
        oldStartDate = startDate;
        startDate = Convert.ToDateTime(value.ToString("d")); // truncate time portion before storing
        isDirty = true;
      }
    }
    /// <summary>
    /// The last date this element is effective
    /// </summary>
    public DateTime EndDate
    {
      get { return endDate; }
      set
      {
        oldEndDate = endDate;
        endDate = Convert.ToDateTime(value.ToString("d"));     // truncate time portion before storing
        isDirty = true;
      }
    }
    /// <summary>
    /// The Entity id of the last user to modify this record
    /// </summary>
    public string LastModifiedBy
    {
      get { return lastModifiedBy; }
      set { lastModifiedBy = value; }
    }
    /// <summary>
    /// The date/time stamp of the last time this record was modified
    /// </summary>
    public DateTime LastModifiedDateTime
    {
      get { return lastModifiedDateTime; }
      set { lastModifiedDateTime = value; }
    }
    public virtual object Clone()
    {
      DateHistory dh = new DateHistory();
      dh.Deleted = Deleted;
      dh.Dirty = Dirty;
      dh.EndDate = EndDate;
      dh.LastModifiedBy = LastModifiedBy;
      dh.LastModifiedDateTime = LastModifiedDateTime;
      dh.StartDate = StartDate;
      return dh;
    }

  }
}
