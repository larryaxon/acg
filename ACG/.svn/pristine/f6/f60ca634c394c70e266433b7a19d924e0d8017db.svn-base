using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAGBOSS.Common.Interface
{
    public interface IDataClassContainer<T> : IDataClassItem, ICloneable
    {
        //    string ID { get; set; }             // the unique ID of the item
        //    bool Dirty { get; set; }            // has the item been changed?
        //    bool Deleted { get; set; }          // is this item flagged as deleted?
        bool MarkForDelete { get; set; }
        string Description { get; set; }
        void Add(T Value);
        void Sort();
        bool Contains(string id);
      T this[string id] { get; set; }
      T this[int index] { get; set; }
      //Dictionaries Dictionary { get; //set; 
      //}
      new string  ToString();
        //    object Clone();                     // Recursively copy the object  
    }
}
