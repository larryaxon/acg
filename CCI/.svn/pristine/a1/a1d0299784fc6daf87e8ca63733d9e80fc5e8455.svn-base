//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Collections;
//using System.Text;
//using CCI.Common;

//namespace CCIWebClient.Common
//{

//    enum Commands : int { getOrderDetail, getOrderHeader, getOrderScreen, getOrderTotals, getPickLists, GetVersion, isValidSecurityID, Login, UpdateOrderDetail, UpdateOrderHeader};
//    enum getOrderDetail: int { id, orderid };
//    enum getOrderHeader : int {id, orderid, shortname, ordername }
//    enum getOrderScreen :int {}
//    enum getOrderTotals:int {orderid}
//    enum getPickList :int {account, context, criteria, fieldnames}
//    enum getVersion :int {}
//    enum isValidSecuriytID :int{securityid}
//    enum Login   :int {username, password, domain}
//    enum UpdateOrderDetails:int {}
//    enum UpdateOrderHeader : int { }
    
//    public class Names
//    {
//        public string[] commands = { "getorderdetail", "getorderheader", "getorderscreen", "getordertotals", "getpicklists", "getversion", "isvalidsecurityid", "login", "updateorderdetail", "updateorderheader" };
//        public string[] getorderdetail = { "id", "orderid" };
//        public string Command

//        public string getOrderDetail(getOrderDetail col)
//        {
//            return getorderdetail[(int)col];
//        }
//    }


//    public class URLHelper2
//    {

//        public static void test()
//        {
            
//            getUrl(CMD.GETORDERHEADER,  
//                    new Parameter(PARAM.GETORDERHEADER.orderId, "VALUE"), 
//                    new Parameter(PARAM.GETORDERHEADER.OrderName, "VALUE")
//                    );
//            }

//         public static Hashtable getUrlParameters(ServerCommands command)
//        {
//             Hashtable ht = new Hashtable();
//            switch (command)
//            {
//                case ServerCommands.GetOrderHeader:
//                    ht.Add(PARAM.GETORDERHEADER.orderId, "");
//                    ht.Add(PARAM.GETORDERHEADER.OrderName, "");
//                    ht.Add(PARAM.GETORDERHEADER.ShortName, "");
//                    return ht;
//                   break;            }
//            return null;
//        }

//        public static string getUrl(ServerCommands command, params Object[] parameters)
//        {
//            Hashtable ht = getUrlParameters(command);
//            StringBuilder sb = new StringBuilder("");
//            foreach (Parameter param in parameters)
//            {
//                if (!ht.ContainsKey(param.name))
//                    throw new Exception("Parameter doesn't exist");
//                else
//                    sb.Append(string.Concat(param.name, "&", param.value));
//            }
            
//            return sb.ToString();
//        }
//    }


    
//    public class Parameter
//    {
//        public string name {get;set;}
//        public string value {get;set;}
//        public Parameter( string name, string value)
//        {
//            this.name = name;
//            this.value = value;
//        }
//    }

//    public static class PARAM {
       
//        public static class GETORDERHEADER
//        {
//            public static   string orderId = "orderid";
//            public static   string ShortName = "shortname";
//            public static   string OrderName = "ordername";
//        }
//    }


    

//}