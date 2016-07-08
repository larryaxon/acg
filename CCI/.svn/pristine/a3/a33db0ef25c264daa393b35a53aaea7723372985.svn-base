using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using CCI.Common;
using CCI.Sys.Data;
using CCI.Common.Logging;
using CCI.Sys.SecurityEngine;

namespace CCI.Sys
{
  public partial class CCIServer
  {
    public ServerResponse GetFieldValidationInfo(ServerRequest request, ServerResponse response)
    {
      CCITable info = _dataSource.getFieldValidationData();
      FieldValidation.load(info);
      response.Results.Add(info);
      return response;
    }
  }
}
