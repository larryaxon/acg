using System;
using System.Collections;

namespace TAGBOSS.Common.Model
{
  public interface ITAGBOSSData
  {
    TEntity[] Entities(
      string[] entities,
      string[] itemTypes,
      Hashtable entitiesRequested,
      Hashtable ItemTypesRequested,
      Hashtable ItemsRequested,
      DateTime effectiveDate,
      bool IsRawMode,
      bool RetrieveVirtualItems);

    TEntity[] SystemEntities(DateTime effectiveDate);

  }
}
