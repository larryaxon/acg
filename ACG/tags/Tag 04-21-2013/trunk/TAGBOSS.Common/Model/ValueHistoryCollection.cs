using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TAGBOSS.Common.Interface;

namespace TAGBOSS.Common.Model
{
  /// <summary>
  /// This is used to instantiate a collection of Value History for an TAGAttribute class
  /// </summary>
  [SerializableAttribute]
  public class ValueHistoryCollection : DataClass<ValueHistory>
  {
    new public object Clone()
    {
      ValueHistoryCollection vh = new ValueHistoryCollection();
      return base.Clone<ValueHistoryCollection>(vh);
    }
    /* 4) Delete Attributes (Deleted = true)To Delete an TAGAttribute, just take its current valuehistory node
    *    and set the End Date to one day less than the effectiveDate or today if that does not exist.
     */
    /// <summary>
    /// Overload that defaults IsNew to True
    /// </summary>
    /// <param name="vh">A ValueHistory object that will be compared with the existing members</param>
    /// <returns>True if there is an overlap, false if not.</returns>
    public bool WillOverlap(ValueHistory valueHistory)
    {
      return WillOverlap(valueHistory, true);
    }

    /// <summary>
    /// Checks to see if a ValueHistory object would overlap in date range with members of the current collection.
    /// This should be called before adding a new valuehistory or updating the enddate of a valuehistory
    /// </summary>
    /// <param name="valueHistory">A ValueHistory object that will be compared with the existing members</param>
    /// <param name="isNew">Is this a new record</param>
    /// <returns>True if there is an overlap, false if not.</returns>
    public bool WillOverlap(ValueHistory valueHistory, bool isNew)
    {
      /* We validate the overlaping looking the sibiling records (prev and next)
       *  1) We must to locate the row which will be updated. (Using valueHistory.EndDate between vh.stardate && vh.endDate)
       *  2) Then we look i there are not overlaping between the previous enddatae and the next startdate
       *  Note: To validate if the startdate is lower than end date you must do it manually.  Single Principle of Responsability
       */

      bool willOverlap = false;
      
      foreach (ValueHistory vh in this)
      {
        if (vh.ID != valueHistory.ID && !(vh.StartDate == valueHistory.StartDate && vh.EndDate == vh.EndDate))
        {
          willOverlap = !((valueHistory.StartDate < vh.StartDate && valueHistory.EndDate < vh.StartDate) || (valueHistory.StartDate > vh.EndDate && valueHistory.EndDate > vh.EndDate));
          if (willOverlap)
            break;
        }
      }
        
      return willOverlap;
    }
    /*
                /// <summary>
                /// Overload of Add that checks if the history segment will create and overlap
                /// </summary>
                /// <param name="pValue"></param>
                public void Add(ValueHistory pValue)
                {
                    if (WillOverlap(pValue))       // assume this is new one if we are using add, so use the WillOverlap that assumes new\
                        throw new DuplicateNameException("Adding this ValueHistory Record would create an overlapped date range");
                    else
                        base.Add(pValue);
                }
    */
  }
}
