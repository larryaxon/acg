using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TAGBOSS.Common;
using TAGBOSS.Common.Interface;

namespace TAGBOSS.Common.Model
{
  [SerializableAttribute]
    /// <summary>
    /// One entry in the InvalidEntries collection
    /// </summary>
    public class InvalidEntry : IDataClassItem          // One entry in the InvalidEntries collection
    {
        private string attributeName = string.Empty;      // also the ID for IDataClass compatibility
        private string context = string.Empty;            // Entity.ItemType.Item or Entity.Fields for now
        private bool deleted = false;                     // supported for DataClass compatibility, but we do not automatically set
        private bool dirty = false;                       // supported for DataClass compatibility, but we do not automatically set
        private string errorMessage = string.Empty;       // error message for failed validation
        private bool readOnly = false;
        //private Dictionaries dictionary = null;

        public Dictionaries Dictionary
        {
          get { return DictionaryFactory.getInstance().getDictionary(); }
          //set { dictionary = value; }
        }
        /// <summary>
        /// is this entry read only?
        /// </summary>
        public bool ReadOnly
        {
            get { return readOnly; }
            set { readOnly = value; }
        }
        /// <summary>
        /// Required to support the IDataClass interface, but a synonym for AttributeName
        /// </summary>
        public string ID                // required to support the IDataClass interface, but a synonym for AttributeName
        {
            get { return attributeName; }
            set { attributeName = value; }
        }
        /// <summary>
        /// Name of the field or TAGAttribute which failed the validation.
        /// </summary>
        public string AttributeName
        {
            get { return attributeName; }
            set { attributeName = value; }
        }
        /// <summary>
        /// Context in which the TAGAttribute or Field that failed validation was found. 
        /// This will be Entity.ItemType.Item or Entity.Fields for now
        /// </summary>
        public string Context
        {
            get { return context; }
            set { context = value; }
        }
        /// <summary>
        /// Error message for failed validation
        /// </summary>
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }
        /// <summary>
        /// Supported for DataClass compatibility, but we do not automatically set it when a value is changed.
        /// </summary>
        public bool Dirty
        {
            get { return dirty; }
            set { dirty = value; }
        }
        /// <summary>
        /// Supported for DataClass compatibility, but not used at this time
        /// </summary>
        public bool Deleted
        {
            get { return deleted; }
            set { deleted = value; }
        }
        public object Clone()
        {
            InvalidEntry ie = new InvalidEntry();
            ie.AttributeName = attributeName;
            ie.Context = context;
            ie.Deleted = deleted;
            ie.Dirty = dirty;
            ie.ErrorMessage = errorMessage;
            return ie;
        }
    }
}
