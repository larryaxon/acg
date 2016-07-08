using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using TAGBOSS.Common.Model;

namespace TAGBOSS.Common
{
  /// <summary>
  /// Container and builder for Parameters to be passed to and consumed by Actions. It contains both
  /// a collection of parameters and a collection of attributes. 
  /// <para>
  /// This has three allowable xml formats:
  /// </para>
  /// <para>
  /// 1) with a root of "parameters", it is identical in format to the Parameters
  /// xml consumed by the Attribute object
  /// </para>
  /// <para>
  /// 2) with a root of "attributes", it is identical in format to the xml in the
  /// attributes column of the attribute table
  /// </para>
  /// <para>
  /// 3) with a root of "actionparameters", it has two nodes. One is a "parameters" node
  /// which is identical to the Attribute Parameters xml. The second is an "attributes" node that is
  /// identical to the xml in the Attributes column of the attribute table.
  /// </para>
  /// </summary>
  public class ActionParameters
  {
    Parameters myParameters;
    Item item;
    /// <summary>
    /// Create an empty instance
    /// </summary>
    public ActionParameters()
    {
      myParameters = new Parameters();
      item = new Item();
    }
    /// <summary>
    /// Create an instance using a standard parameters xml string and an Item containing attributes
    /// </summary>
    /// <param name="xmlParameters">Standard parameter xml string</param>
    /// <param name="myItem">Item containing a list of attributes</param>
    public ActionParameters(string xmlParameters, Item myItem)
    {
      myParameters = new Parameters(xmlParameters);
      if (myItem == null)
        item = new Item();
      else
        item = myItem;
    }
    /// <summary>
    /// Create an instance using a Parameters object and an Item object
    /// </summary>
    /// <param name="myParms">Parameters object containing the list of parameters</param>
    /// <param name="myItem">Item containing a list of attributes</param>
    public ActionParameters(Parameters myParms, Item myItem)
    {
      if (myParms == null)
        myParameters = new Parameters();
      else
        myParameters = myParms;
      if (myItem == null)
        item = new Item();
      else
        item = myItem;
    }
    /// <summary>
    /// Create an instance from a "big XML" with a root node of actionparameters, and 
    /// two daughter nodes of parameters and attributes. This also accepts just parameters
    /// in the Attribute parameters format without the actionparameters node above it.
    /// </summary>
    /// <param name="xmlActionParameters"></param>
    public ActionParameters(string xmlActionParameters)
    {
      ItemType it = new ItemType();
      XmlDocument bigXml = new XmlDocument();
      bigXml.LoadXml(xmlActionParameters);
      XmlNode root = bigXml.DocumentElement;
      if (root.Name.Equals("actionparameters"))
      {
        try
        {
          foreach (XmlNode node in root.ChildNodes)
          {
            bool exitForEach = false;
            switch (node.Name.ToLower())
            {
              case "parameters":
                myParameters = new Parameters(node.OuterXml);
                break;
              case "attributes":
                item = it.AttributesCollection(node.OuterXml, DateTime.Today);
                break;
              default:
                /*
                 * this node does not have the parameters or attributes section, 
                 * so we just ignore it
                 */

                break;
            }
          }
        }
        catch (Exception e)
        {
          string x = e.Message;
        }
      }
      else
      {
        item = new Item();
        if (root.Name.ToLower() == "parameters")
        {
          myParameters = new Parameters(xmlActionParameters);
        }
        else
        {
          /*
           * the main node name is bad, so we just create an empty set
           */
          myParameters = new Parameters();
        }
      }
    }
    /// <summary>
    /// Returns the value of a parameter. This does not return the value of an attribute
    /// </summary>
    /// <param name="parmName"></param>
    /// <returns></returns>
    public string this[string parmName]
    {
      get { return myParameters[parmName]; }
      set { myParameters[parmName] = value; }
    }
    /// <summary>
    /// read-only version of an Item object which contains an attributes collection. This
    /// can be used to access an attribute using the syntax:
    /// <para>
    /// TAGAttribute a = myActionParameters.Attributes[attributeName];
    /// </para>
    /// </summary>
    public Item Attributes
    {
      get { return item; }
    }
    /// <summary>
    /// read-only version of the imbedde Parameters object. This can be used to access
    /// a parameter value using the syntax:
    /// <para>
    /// string parmValue = myActionParameters.Parms[parameterName];
    /// </para>
    /// </summary>
    public Parameters Parms
    {
      get { return myParameters; }
    }
    /// <summary>
    /// Add a new parameter to the collection
    /// </summary>
    /// <param name="parameterName"></param>
    /// <param name="parameterValue"></param>
    /// <param name="dataType"></param>
    public void AddParameter(string parameterName, string parameterValue, string dataType)
    {
      if (myParameters == null)
        myParameters = new Parameters();
      myParameters.Add(parameterName, parameterValue, dataType);
    }
    public void AddParameter(string parameterName, string parameterValue)
    {
      AddParameter(parameterName, parameterValue, null);
    }
    public void AddAttribute(string pID, object pValue)
    {
      if (item == null)
        item = new Item();
      item.AddAttribute(pID, pValue);
    }
    public void AddAttribute(string pID, object pValue, string dataType)
    {
      if (item == null)
        item = new Item();
      item.AddAttribute(pID, pValue, dataType);
    }
    public string ToXML()
    {
        string xmlBegin = "<actionparameters>";
        string xmlEnd = "</actionparameters>";
        string xmlOut = xmlBegin;
        if (myParameters != null)
            xmlOut += myParameters.ToXML();
        if (item != null)
            xmlOut += item.ToXML("", true);
        xmlOut += xmlEnd;
        return xmlOut;
        
        //string xmlBegin = "<actionparameters>\n";
        //string xmlEnd = "</actionparameters>";
        //string xmlOut = xmlBegin;
        //if (myParameters != null)
        //    xmlOut += myParameters.ToXML() + "\n";
        //if (item != null)
        //    xmlOut += item.ToXML("", true) + "\n";
        //xmlOut += xmlEnd;
        //return xmlOut;
    }
  }
}
