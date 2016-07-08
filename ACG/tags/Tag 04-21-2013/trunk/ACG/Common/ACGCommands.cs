using System;
using System.Collections.Generic;
using System.Text;

namespace ACG.Common
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
