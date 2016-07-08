using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Interop.QBFC12;

namespace InvoiceAdd
{
    public partial class ImportInvoices : Form
    {
        public ImportInvoices()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "Select file";
            fdlg.InitialDirectory = @"c:\";
            fdlg.FileName = txtFileName.Text;
            fdlg.Filter = "Text and CSV Files(*.txt, *.csv)|*.txt;*.csv|Text Files(*.txt)|*.txt|CSV Files(*.csv)|*.csv|All Files(*.*)|*.*";
            fdlg.FilterIndex = 1;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                txtFileName.Text = fdlg.FileName;
                Import();
                Application.DoEvents();
            }
        }

        public static DataTable GetDataTable(string strFileName)
        {
            ADODB.Connection oConn = new ADODB.Connection();
            oConn.Open("Provider=Microsoft.Jet.OleDb.4.0; Data Source = " + System.IO.Path.GetDirectoryName(strFileName) + "; Extended Properties = 'Text;HDR=NO;FMT=Delimited';", "", "", 0);
            string strQuery = "SELECT * FROM [" + System.IO.Path.GetFileName(strFileName) + "]";
            ADODB.Recordset rs = new ADODB.Recordset();
            System.Data.OleDb.OleDbDataAdapter adapter = new System.Data.OleDb.OleDbDataAdapter();
            DataTable dt = new DataTable();
            rs.Open(strQuery, "Provider=Microsoft.Jet.OleDb.4.0; Data Source = " + System.IO.Path.GetDirectoryName(strFileName) + "; Extended Properties = 'Text;HDR=NO;FMT=Delimited';",
                ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly, 1);
            adapter.Fill(dt, rs);
            return dt;
        }

        private void Import()
        {
            if (txtFileName.Text.Trim() != string.Empty)
            {
                try
                {
                    DataTable dt = GetDataTable(txtFileName.Text);
                    dgvInvoices.DataSource = dt.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        // Code for handling different versions of QuickBooks
        private double QBFCLatestVersion(QBSessionManager SessionManager)
        {
            // Use oldest version to ensure that this application work with any QuickBooks (US)
            IMsgSetRequest msgset = SessionManager.CreateMsgSetRequest("US", 1, 0);
            msgset.AppendHostQueryRq();
            IMsgSetResponse QueryResponse = SessionManager.DoRequests(msgset);
            //MessageBox.Show("Host query = " + msgset.ToXMLString());
            //SaveXML(msgset.ToXMLString());


            // The response list contains only one response,
            // which corresponds to our single HostQuery request
            IResponse response = QueryResponse.ResponseList.GetAt(0);

            // Please refer to QBFC Developers Guide for details on why 
            // "as" clause was used to link this derrived class to its base class
            IHostRet HostResponse = response.Detail as IHostRet;
            IBSTRList supportedVersions = HostResponse.SupportedQBXMLVersionList as IBSTRList;

            int i;
            double vers;
            double LastVers = 0;
            string svers = null;

            for (i = 0; i <= supportedVersions.Count - 1; i++)
            {
                svers = supportedVersions.GetAt(i);
                vers = Convert.ToDouble(svers);
                if (vers > LastVers)
                {
                    LastVers = vers;
                }
            }
            return LastVers;
        }

        public IMsgSetRequest getLatestMsgSetRequest(QBSessionManager sessionManager)
        {
            // Find and adapt to supported version of QuickBooks
            double supportedVersion = QBFCLatestVersion(sessionManager);

            short qbXMLMajorVer = 0;
            short qbXMLMinorVer = 0;

            if (supportedVersion >= 6.0)
            {
                qbXMLMajorVer = 6;
                qbXMLMinorVer = 0;
            }
            else if (supportedVersion >= 5.0)
            {
                qbXMLMajorVer = 5;
                qbXMLMinorVer = 0;
            }
            else if (supportedVersion >= 4.0)
            {
                qbXMLMajorVer = 4;
                qbXMLMinorVer = 0;
            }
            else if (supportedVersion >= 3.0)
            {
                qbXMLMajorVer = 3;
                qbXMLMinorVer = 0;
            }
            else if (supportedVersion >= 2.0)
            {
                qbXMLMajorVer = 2;
                qbXMLMinorVer = 0;
            }
            else if (supportedVersion >= 1.1)
            {
                qbXMLMajorVer = 1;
                qbXMLMinorVer = 1;
            }
            else
            {
                qbXMLMajorVer = 1;
                qbXMLMinorVer = 0;
                MessageBox.Show("It seems that you are running QuickBooks 2002 Release 1. We strongly recommend that you use QuickBooks' online update feature to obtain the latest fixes and enhancements");
            }

            // Create the message set request object
            IMsgSetRequest requestMsgSet = sessionManager.CreateMsgSetRequest("US", qbXMLMajorVer, qbXMLMinorVer);
            return requestMsgSet;
        }

        public enum CELLS : int
        {
            DATE=0,
            CUSTOMER,
            MRC,
            OCC,
            TOLL,
            TAX,
            INTERNAL
        };


        public void QBFC_AddInvoice()
        {
            IMsgSetRequest requestMsgSet;
            IMsgSetResponse responseMsgSet;
            // Create the session manager object using QBFC
            QBSessionManager sessionManager = new QBSessionManager();

            // We want to know if we begun a session so we can end it if an
            // error happens
            bool booSessionBegun = false;

            try
            {
                // Use SessionManager object to open a connection and begin a session 
                // with QuickBooks. At this time, you should add interop.QBFCxLib into 
                // your Project References
                //sessionManager.OpenConnection("", "AGC QBInvoiceAdd");
                sessionManager.OpenConnection2("", "AGC QBInvoiceAdd", ENConnectionType.ctLocalQBD);

                sessionManager.BeginSession("", ENOpenMode.omDontCare);
                booSessionBegun = true;

                // Get the RequestMsgSet based on the correct QB Version
                requestMsgSet = getLatestMsgSetRequest(sessionManager);
                // requestMsgSet = sessionManager.CreateMsgSetRequest("US", 4, 0);

                // Initialize the message set request object
                requestMsgSet.Attributes.OnError = ENRqOnError.roeContinue;

                // ERROR RECOVERY: 
                // All steps are described in QBFC Developers Guide, on pg 41
                // under section titled "Automated Error Recovery"

                // (1) Set the error recovery ID using ErrorRecoveryID function
                //		Value must be in GUID format
                //	You could use c:\Program Files\Microsoft Visual Studio\Common\Tools\GuidGen.exe 
                //	to create a GUID for your unique ID
                string errecid = "{E74068B5-0D6D-454d-B0FD-BDDF93CE67C3}";
                sessionManager.ErrorRecoveryID.SetValue(errecid);

                // (2) Set EnableErrorRecovery to true to enable error recovery
                sessionManager.EnableErrorRecovery = true;

                // (3) Set SaveAllMsgSetRequestInfo to true so the entire contents of the MsgSetRequest
                //		will be saved to disk. If SaveAllMsgSetRequestInfo is false (default), only the 
                //		newMessageSetID will be saved. 
                sessionManager.SaveAllMsgSetRequestInfo = true;

                // (4) Use IsErrorRecoveryInfo to check whether an unprocessed response exists. 
                //		If IsErrorRecoveryInfo is true:
                if (sessionManager.IsErrorRecoveryInfo())
                {
                    //string reqXML;
                    //string resXML;
                    IMsgSetRequest reqMsgSet = null;
                    IMsgSetResponse resMsgSet = null;

                    // a. Get the response status, using GetErrorRecoveryStatus
                    resMsgSet = sessionManager.GetErrorRecoveryStatus();
                    // resXML = resMsgSet.ToXMLString();
                    // MessageBox.Show(resXML);

                    if (resMsgSet.Attributes.MessageSetStatusCode.Equals("600"))
                    {
                        // This case may occur when a transaction has failed after QB processed 
                        // the request but client app didn't get the response and started with 
                        // another company file.
                        MessageBox.Show("The oldMessageSetID does not match any stored IDs, and no newMessageSetID is provided.");
                    }
                    else if (resMsgSet.Attributes.MessageSetStatusCode.Equals("9001"))
                    {
                        MessageBox.Show("Invalid checksum. The newMessageSetID specified, matches the currently stored ID, but checksum fails.");
                    }
                    else if (resMsgSet.Attributes.MessageSetStatusCode.Equals("9002"))
                    {
                        // Response was not successfully stored or stored properly
                        MessageBox.Show("No stored response was found.");
                    }
                    // 9003 = Not used
                    else if (resMsgSet.Attributes.MessageSetStatusCode.Equals("9004"))
                    {
                        // MessageSetID is set with a string of size > 24 char
                        MessageBox.Show("Invalid MessageSetID, greater than 24 character was given.");
                    }
                    else if (resMsgSet.Attributes.MessageSetStatusCode.Equals("9005"))
                    {
                        MessageBox.Show("Unable to store response.");
                    }
                    else
                    {
                        IResponse res = resMsgSet.ResponseList.GetAt(0);
                        int sCode = res.StatusCode;
                        //string sMessage = res.StatusMessage;
                        //string sSeverity = res.StatusSeverity;
                        //MessageBox.Show("StatusCode = " + sCode + "\n" + "StatusMessage = " + sMessage + "\n" + "StatusSeverity = " + sSeverity);

                        if (sCode == 0)
                        {
                            MessageBox.Show("Last request was processed and Invoice was added successfully!");
                        }
                        else if (sCode > 0)
                        {
                            MessageBox.Show("There was a warning but last request was processed successfully!");
                        }
                        else
                        {
                            MessageBox.Show("It seems that there was an error in processing last request");
                            // b. Get the saved request, using GetSavedMsgSetRequest
                            reqMsgSet = sessionManager.GetSavedMsgSetRequest();
                            //reqXML = reqMsgSet.ToXMLString();
                            //MessageBox.Show(reqXML);

                            // c. Process the response, possibly using the saved request
                            resMsgSet = sessionManager.DoRequests(reqMsgSet);
                            IResponse resp = resMsgSet.ResponseList.GetAt(0);
                            int statCode = resp.StatusCode;
                            if (statCode == 0)
                            {
                                string resStr = null;
                                IInvoiceRet invRet = resp.Detail as IInvoiceRet;
                                resStr = resStr + "Following invoice has been successfully submitted to QuickBooks:\n\n\n";
                                if (invRet.TxnNumber != null)
                                    resStr = resStr + "Txn Number = " + Convert.ToString(invRet.TxnNumber.GetValue()) + "\n";
                            } // if (statusCode == 0)
                        } // else (sCode)
                    } // else (MessageSetStatusCode)

                    // d. Clear the response status, using ClearErrorRecovery
                    sessionManager.ClearErrorRecovery();
                    MessageBox.Show("Proceeding with current transaction.");                 
                }

                // Add the request to the message set request object
                IInvoiceAdd invoiceAdd;// = requestMsgSet.AppendInvoiceAddRq();

                // Set the IInvoiceAdd field values
                // Customer:Job
                if ((dgvInvoices.RowCount) == 0)
                {
                    MessageBox.Show("Nothing to process.  Please select a file to send.");
                    return;
                }

                for (int nRow = 0; nRow < (dgvInvoices.RowCount)-1; nRow++)
                {
                    invoiceAdd = requestMsgSet.AppendInvoiceAddRq();


                    // Invoice Date                    
                    string invoiceDate = dgvInvoices.Rows[nRow].Cells[(int)CELLS.DATE].Value.ToString();
                    if (!invoiceDate.Equals(""))
                    {
                        invoiceAdd.TxnDate.SetValue(Convert.ToDateTime(invoiceDate));
                    }
                    // Customer:Job
                    string customer = dgvInvoices.Rows[nRow].Cells[(int)CELLS.CUSTOMER].Value.ToString();
                    if (!customer.Equals(""))
                    {
                        invoiceAdd.CustomerRef.FullName.SetValue(customer);
                    }
                    invoiceAdd.TermsRef.FullName.SetValue("Net 20");
                    // Add line(s)
                    IInvoiceLineAdd invoiceLineAdd = invoiceAdd.ORInvoiceLineAddList.Append().InvoiceLineAdd;
                    string item = "MRC";
                    invoiceLineAdd.ItemRef.FullName.SetValue(item);
                    string amount = dgvInvoices.Rows[nRow].Cells[(int)CELLS.MRC].Value.ToString();
                    invoiceLineAdd.Amount.SetValue(Convert.ToDouble(amount));
                    //string taxable = "N";
                    // Currently IsTaxable is not supported in QBD - QuickBooks Desktop Edition
                    /*
                    if (taxable.ToUpper().Equals("Y") || taxable.ToUpper().Equals("N"))
                    {
                        bool isTaxable = false;
                        if (taxable.ToUpper().Equals("Y")) isTaxable=true;
                            invoiceLineAdd.IsTaxable.SetValue(isTaxable);
                    }
                    */

                    invoiceLineAdd = invoiceAdd.ORInvoiceLineAddList.Append().InvoiceLineAdd;
                    item = "OCC";
                    invoiceLineAdd.ItemRef.FullName.SetValue(item);
                    amount = dgvInvoices.Rows[nRow].Cells[(int)CELLS.OCC].Value.ToString();
                    invoiceLineAdd.Amount.SetValue(Convert.ToDouble(amount));
                    //taxable = "N";

                    invoiceLineAdd = invoiceAdd.ORInvoiceLineAddList.Append().InvoiceLineAdd;
                    item = "TOLL";
                    invoiceLineAdd.ItemRef.FullName.SetValue(item);
                    amount = dgvInvoices.Rows[nRow].Cells[(int)CELLS.TOLL].Value.ToString();
                    invoiceLineAdd.Amount.SetValue(Convert.ToDouble(amount));
                    //taxable = "N";

                    invoiceLineAdd = invoiceAdd.ORInvoiceLineAddList.Append().InvoiceLineAdd;
                    item = "TAX";
                    invoiceLineAdd.ItemRef.FullName.SetValue(item);
                    amount = dgvInvoices.Rows[nRow].Cells[(int)CELLS.TAX].Value.ToString();
                    invoiceLineAdd.Amount.SetValue(Convert.ToDouble(amount));
                    //taxable = "N";

                }

                // Uncomment the following to view and save the request and response XML
                //string requestXML = requestMsgSet.ToXMLString();
                //MessageBox.Show(requestXML);
                //SaveXML(requestXML, "RQ");

                // Perform the request and obtain a response from QuickBooks
                responseMsgSet = sessionManager.DoRequests(requestMsgSet);

                // Uncomment the following to view and save the request and response XML
                string requestXML = requestMsgSet.ToXMLString();
                //MessageBox.Show(requestXML);
                SaveXML(requestXML, "RQInvoice_");
                string responseXML = responseMsgSet.ToXMLString();
                //MessageBox.Show(responseXML);
                SaveXML(responseXML, "RSInvoice_");

                IResponse response = responseMsgSet.ResponseList.GetAt(0);
                int statusCode = response.StatusCode;
                // string statusMessage = response.StatusMessage;
                // string statusSeverity = response.StatusSeverity;
                // MessageBox.Show("Status:\nCode = " + statusCode + "\nMessage = " + statusMessage + "\nSeverity = " + statusSeverity);

                if (statusCode == 0)
                {
                    string resString = null;
                    IInvoiceRet invoiceRet = response.Detail as IInvoiceRet;
                    resString = resString + "Following invoice has been successfully submitted to QuickBooks:\n\n\n";
                    if (invoiceRet.TimeCreated != null)
                        resString = resString + "Time Created = " + Convert.ToString(invoiceRet.TimeCreated.GetValue()) + "\n";
                    if (invoiceRet.TxnNumber != null)
                        resString = resString + "Txn Number = " + Convert.ToString(invoiceRet.TxnNumber.GetValue()) + "\n";
                    if (invoiceRet.TxnDate != null)
                        resString = resString + "Txn Date = " + Convert.ToString(invoiceRet.TxnDate.GetValue()) + "\n";
                    if (invoiceRet.RefNumber != null)
                        resString = resString + "Reference Number = " + invoiceRet.RefNumber.GetValue() + "\n";
                    if (invoiceRet.CustomerRef.FullName != null)
                        resString = resString + "Customer FullName = " + invoiceRet.CustomerRef.FullName.GetValue() + "\n";
                    resString = resString + "\nBilling Address:" + "\n";
                    if (invoiceRet.BillAddress.Addr1 != null)
                        resString = resString + "Addr1 = " + invoiceRet.BillAddress.Addr1.GetValue() + "\n";
                    if (invoiceRet.BillAddress.Addr2 != null)
                        resString = resString + "Addr2 = " + invoiceRet.BillAddress.Addr2.GetValue() + "\n";
                    if (invoiceRet.BillAddress.Addr3 != null)
                        resString = resString + "Addr3 = " + invoiceRet.BillAddress.Addr3.GetValue() + "\n";
                    if (invoiceRet.BillAddress.Addr4 != null)
                        resString = resString + "Addr4 = " + invoiceRet.BillAddress.Addr4.GetValue() + "\n";
                    if (invoiceRet.BillAddress.City != null)
                        resString = resString + "City = " + invoiceRet.BillAddress.City.GetValue() + "\n";
                    if (invoiceRet.BillAddress.State != null)
                        resString = resString + "State = " + invoiceRet.BillAddress.State.GetValue() + "\n";
                    if (invoiceRet.BillAddress.PostalCode != null)
                        resString = resString + "Postal Code = " + invoiceRet.BillAddress.PostalCode.GetValue() + "\n";
                    if (invoiceRet.BillAddress.Country != null)
                        resString = resString + "Country = " + invoiceRet.BillAddress.Country.GetValue() + "\n";
                    if (invoiceRet.PONumber != null)
                        resString = resString + "\nPO Number = " + invoiceRet.PONumber.GetValue() + "\n";
                    if (invoiceRet.TermsRef.FullName != null)
                        resString = resString + "Terms = " + invoiceRet.TermsRef.FullName.GetValue() + "\n";
                    if (invoiceRet.DueDate != null)
                        resString = resString + "Due Date = " + Convert.ToString(invoiceRet.DueDate.GetValue()) + "\n";
                    if (invoiceRet.SalesTaxTotal != null)
                        resString = resString + "Sales Tax = " + Convert.ToString(invoiceRet.SalesTaxTotal.GetValue()) + "\n";
                    resString = resString + "\nInvoice Line Items:" + "\n";
                    IORInvoiceLineRetList orInvoiceLineRetList = invoiceRet.ORInvoiceLineRetList;
                    string fullname = "<empty>";
                    string desc = "<empty>";
                    string rate = "<empty>";
                    string quantity = "<empty>";
                    string amount = "<empty>";
                    for (int i = 0; i <= orInvoiceLineRetList.Count - 1; i++)
                    {
                        if (invoiceRet.ORInvoiceLineRetList.GetAt(i).ortype == ENORInvoiceLineRet.orilrInvoiceLineRet)
                        {
                            if (invoiceRet.ORInvoiceLineRetList.GetAt(i).InvoiceLineRet.ItemRef.FullName != null)
                                fullname = invoiceRet.ORInvoiceLineRetList.GetAt(i).InvoiceLineRet.ItemRef.FullName.GetValue();
                            if (invoiceRet.ORInvoiceLineRetList.GetAt(i).InvoiceLineRet.Desc != null)
                                desc = invoiceRet.ORInvoiceLineRetList.GetAt(i).InvoiceLineRet.Desc.GetValue();
                            if (invoiceRet.ORInvoiceLineRetList.GetAt(i).InvoiceLineRet.ORRate.Rate != null)
                                rate = Convert.ToString(invoiceRet.ORInvoiceLineRetList.GetAt(i).InvoiceLineRet.ORRate.Rate.GetValue());
                            if (invoiceRet.ORInvoiceLineRetList.GetAt(i).InvoiceLineRet.Quantity != null)
                                quantity = Convert.ToString(invoiceRet.ORInvoiceLineRetList.GetAt(i).InvoiceLineRet.Quantity.GetValue());
                            if (invoiceRet.ORInvoiceLineRetList.GetAt(i).InvoiceLineRet.Amount != null)
                                amount = Convert.ToString(invoiceRet.ORInvoiceLineRetList.GetAt(i).InvoiceLineRet.Amount.GetValue());
                        }
                        else
                        {
                            if (invoiceRet.ORInvoiceLineRetList.GetAt(i).InvoiceLineGroupRet.ItemGroupRef.FullName != null)
                                fullname = invoiceRet.ORInvoiceLineRetList.GetAt(i).InvoiceLineGroupRet.ItemGroupRef.FullName.GetValue();
                            if (invoiceRet.ORInvoiceLineRetList.GetAt(i).InvoiceLineGroupRet.Desc != null)
                                desc = invoiceRet.ORInvoiceLineRetList.GetAt(i).InvoiceLineGroupRet.Desc.GetValue();
                            if (invoiceRet.ORInvoiceLineRetList.GetAt(i).InvoiceLineGroupRet.InvoiceLineRetList.GetAt(i).ORRate.Rate != null)
                                rate = Convert.ToString(invoiceRet.ORInvoiceLineRetList.GetAt(i).InvoiceLineGroupRet.InvoiceLineRetList.GetAt(i).ORRate.Rate.GetValue());
                            if (invoiceRet.ORInvoiceLineRetList.GetAt(i).InvoiceLineGroupRet.Quantity != null)
                                quantity = Convert.ToString(invoiceRet.ORInvoiceLineRetList.GetAt(i).InvoiceLineGroupRet.Quantity.GetValue());
                            if (invoiceRet.ORInvoiceLineRetList.GetAt(i).InvoiceLineGroupRet.TotalAmount != null)
                                amount = Convert.ToString(invoiceRet.ORInvoiceLineRetList.GetAt(i).InvoiceLineGroupRet.TotalAmount.GetValue());
                        }
                        resString = resString + "Fullname: " + fullname + "\n";
                        resString = resString + "Description: " + desc + "\n";
                        resString = resString + "Rate: " + rate + "\n";
                        resString = resString + "Quantity: " + quantity + "\n";
                        resString = resString + "Amount: " + amount + "\n\n";
                    }
                    //MessageBox.Show(resString);
                    //SaveXML(resString);
                    MessageBox.Show("The invoices were successfully sent to QuickBooks!");
                    dgvInvoices.DataSource = null;
                    dgvInvoices.Refresh();
                }
                else
                {
                    MessageBox.Show("There were errors sending received payments sent to QuickBooks.");

                } // if statusCode is zero
            }
            // Close the session and connection with QuickBooks
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "\nStack Trace: \n" + ex.StackTrace + "\nExiting the application");
                if (booSessionBegun)
                {
                    sessionManager.EndSession();
                    sessionManager.CloseConnection();
                }
                Application.Exit();
            }
            sessionManager.ClearErrorRecovery();
            sessionManager.EndSession();
            booSessionBegun = false;
            sessionManager.CloseConnection();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            QBFC_AddInvoice();
        } 

        void SaveXML(string xmlstring, string prepend)
        {
            string fn = ".\\"+prepend+DateTime.Now.ToString("dd-MM-yyyy HH.mm.ss");
            //if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            //{
                //StreamWriter sr = new StreamWriter(saveFileDialog1.FileName);
                StreamWriter sr = new StreamWriter(fn);
                sr.Write(xmlstring);
                sr.Close();
            //}
        }

        void SaveXML(string xmlstring)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
            StreamWriter sr = new StreamWriter(saveFileDialog1.FileName);
            sr.Write(xmlstring);
            sr.Close();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.Run(new frm1_InvoiceAdd());
            Application.Run(new ImportInvoices());
        }
    }

}
