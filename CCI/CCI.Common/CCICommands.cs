using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCI.Common
{
  public enum ServerCommands
  {
    Login,
    GetCustomerInfo,
    GetCustomerSuggestions,
    GetFieldValidationInfo,
    GetOrderDetail,
    GetOrderHeader,
    GetOrderScreen,
    GetOrderTotals,
    IsValidSecurityID,
    GetPickLists,
    GetVersion,
    UpdateOrderDetail,
    UpdateOrderHeader,
    None
  };
}
