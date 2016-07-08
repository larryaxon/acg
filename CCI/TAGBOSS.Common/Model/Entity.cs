using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAGBOSS.Common.Model
{
  /// <summary>
  /// The Entity class. This is a collection of both ItemTypes and Fields. 
  /// </summary>
  [SerializableAttribute] 
  public class Entity : FieldsCollection   // defines the Entity Class, inheriting the Fields collection from DataRecord
  {
    private ItemTypeCollection itemTypes = new ItemTypeCollection();// and adding storage for the Attributes (via ItemTypes) collection
    private bool isNew = false;

    public bool IsNew
    {
      get { return isNew; }
      set { isNew = value; }
    }
    public new Dictionaries Dictionary
    {
      get { return DictionaryFactory.getInstance().getDictionary(); }
      //set
      //{
      //  if (itemTypes.Dictionary == null || value == null)
      //    itemTypes.Dictionary = value;
      //  base.Dictionary = value;
      //}
    }
    /// <summary>
    /// Overload for entities that looks in ItemTypesHas this collection been modified?
    /// </summary>
    public new bool Dirty
    {
      get { return (itemTypes.Dirty || base.Dirty); }   // if either the base (which includes Fields collection) or ItemTypes is dirty, then return dirty
      set
      {
        if (!value)               // if we are resetting the dirty flag to "clean"
        {
          itemTypes.Dirty = value;
          //foreach (ItemType it in ItemTypes)
          //  it.Dirty = false;    // then set all of the children to clean, too
          foreach (Field f in Fields)
            f.Dirty = false;
        }
        isDirty = value;            // set this instance to the value passed in
      }
    }
    /// <summary>
    /// A collection of ItemTypes in the Entity class. Since Entity has both Fields and ItemTypes, and since Entity 
    /// is inherited from DataRecord which automatically makes it a collection of Fields, then we need to create the storage
    /// above and the collection below to allow it to contain both collections.
    /// </summary>
    public ItemTypeCollection ItemTypes                             // ItemTypes is unique alias to this class for the collection of ItemTypes
    {
      get { return itemTypes; }
      set { itemTypes = value; }
    }
    public string EntityType
    {
      get { return (string)TAGFunctions.evaluateFunction(TAGFunctions.EnumFunctions.CString, getValue("EntityType")); }
      set { setValue("EntityType", value); }
    }
    /// <summary>
    /// Returns a copy of the current object. All children are also cloned, so that every copy of every object 
    /// is brand new.      
    /// </summary>
    /// <returns>A new copy of the object</returns>
    public new object Clone()
    {
      Entity e = new Entity();
      foreach (Field f in Fields)
      {
        Field fNew = (Field)f.Clone();
        e.Fields.Add(fNew);
      }
      e.Deleted = Deleted;
      e.Dirty = Dirty;
      e.MarkForDelete = MarkForDelete;
      e.ID = ID;
      e.Description = Description;
      e.ItemTypes = (ItemTypeCollection)ItemTypes.Clone();
      return e;
    }
  }
}
