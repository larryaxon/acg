using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChargifyNET;
using ACG.Common;

namespace CCI.Common
{
  public class Chargify
  {
    private string _urlConfig = null;
    private string _apiKeyConfig = null;
    private string _passwordConfig = null;
    private string _sharedKeyConfig = null;
    private string _startupComponentIDConfig = null;
    private ChargifyConnect _chargifyConnect = null;
    private string _url { get { if (_urlConfig == null) _urlConfig = CommonFunctions.getAppSetting("ChargifyURL"); return _urlConfig; } }
    private string _apiKey { get { if (_apiKeyConfig == null) _apiKeyConfig = CommonFunctions.getAppSetting("ChargifyAPIKey"); return _apiKeyConfig; } }
    private string _password { get { if (_passwordConfig == null) _passwordConfig = CommonFunctions.getAppSetting("ChargifyPassword"); return _passwordConfig; } }
    private string _sharedKey { get { if (_sharedKeyConfig == null) _sharedKeyConfig = CommonFunctions.getAppSetting("ChargifySharedKey"); return _sharedKeyConfig; } }
    private string _startupComponentID { get { if (_startupComponentIDConfig == null) _startupComponentIDConfig = CommonFunctions.getAppSetting("ChargifySetupComponent"); return _startupComponentIDConfig; } }
    private ChargifyConnect _chargify { get { if (_chargifyConnect == null) _chargifyConnect = new ChargifyConnect(_url, _apiKey, _password); return _chargifyConnect; } }

    public Chargify()
    {
      
    }

    public ISubscription getSubscription(int subscriptionID)
    {
      return _chargify.LoadSubscription(subscriptionID);
    }
    //public ICustomer getCustomer(ISubscription subscription)
    //{
    //  return subscription.Customer;
    //}
    public ICustomer getCustomer(int customerID)
    {
      return _chargify.LoadCustomer(customerID);
    }
    public void resetStartupComponent(int subscriptionID)
    {
      _chargify.UpdateComponentAllocationForSubscription(subscriptionID, CommonFunctions.CInt(_startupComponentID), 0);
    }
    public void createMonthlyCharge(int subscriptionID, decimal amount, string memo)
    {
        // chargify.CreateCharge(<subscriptionId>,<amount>,<memo>, useNegativeBalance, delayCapture);
        // useNegativeBalance = apply any credit on the subscription to this charge
        // delayCapture = charge at next renewal as opposed to immediately
        if (string.IsNullOrEmpty(memo))
           memo = "Monthly Charge";
        _chargify.CreateCharge(subscriptionID,amount,memo, true, true);    
    }
    public void getChargifyCustomerStatus(int subscriptionID)
    {

	    ISubscription subscription = _chargify.LoadSubscription(subscriptionID);
  //activeCustomer.subscriptionState = subscription.State;
  //activeCustomer.balance = subscription.Balance;
  //if (activeCustomer.subscriptionState == ‘active’ )
  //    {// Customer has successfully been charged
  //    }
  //else if (activeCustomer.subscriptionState == ‘past_due’ )
  //      {// Customer is in Dunning cycle, payment has failed to date
  //      }
  //else if (activeCustomer.subscriptionState == ‘unpaid’ )
  //        {// Dunning process has completed without successful payment}
	
  //activeCustomer.save();
// Get transactions since last retrieval
// GetTransactionsForSubscription* methods available
// Suggest storing last transaction id on customer record, and adding 1 for next retrieval	
    
    }
  }
}
