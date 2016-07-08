using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAGBOSS.Common;

namespace TAGBOSS.Common.Interface
{
    /// <summary>
    /// This interface is required to be implemented by any collection item that is to be instantiated into a DataClass collection.
    /// It guarantees that certain properties and methods of DataClass will work properly.
    /// </summary>
    public interface IDataClassItem : ICloneable
    {
        /*
         * This interface is required to be implemented by any item that is to be collected by a DataClass collection
         * 
         * It guarantees that certain properties and methods of DataClass will work properly
         * 
         */
        string ID { get; set; }             // the unique ID of the item
        bool Dirty { get; set; }            // has the item been changed?
        bool Deleted { get; set; }          // is this item flagged as deleted?
        bool ReadOnly { get; set; }         // is this item flagged as read-only?
        Dictionaries Dictionary { get; //set; 
        }
        string ToString();
        //object Clone();                     // Recursively copy the object
    }
}
