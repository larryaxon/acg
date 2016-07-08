using System;
using System.Text;
using ACG.Common;

using NUnit.Framework;


namespace ATK_NUNITs
{
  [TestFixture]
  public class CommonFunctionsTests
  {
    #region parsing function tests!

    StringBuilder JsonResultsTest = new StringBuilder();
    StringBuilder JsonFormTest = new StringBuilder();
    StringBuilder JsonTimeKeeperTest = new StringBuilder();
    StringBuilder JsonContactTest = new StringBuilder();
    StringBuilder JsonMatterTest = new StringBuilder();
    StringBuilder JsonClientTest = new StringBuilder();
    StringBuilder JsonAccountTest = new StringBuilder();
    StringBuilder JsonSecurityContextTest = new StringBuilder();
    StringBuilder JsonTimeOptionsTest = new StringBuilder();
    StringBuilder JsonErrorsTest = new StringBuilder();

    [TestFixtureSetUp]
    public void initService()
    {
    //  JsonResultsTest.Append("{ ");
    //  JsonResultsTest.Append(" \"results\": ");
    //  JsonResultsTest.Append("  { ");

    //  JsonFormTest.Append("    \"form\": \"\" ");
    //  JsonFormTest.Append("    { ");

    //  JsonTimeKeeperTest.Append("      \"timekeeper\": ");
    //  JsonTimeKeeperTest.Append("      { ");
    //  JsonTimeKeeperTest.Append("        \"type\": \"PickList\", ");
    //  JsonTimeKeeperTest.Append("        \"value\": \"table\": ");
    //  JsonTimeKeeperTest.Append("        { ");
    //  JsonTimeKeeperTest.Append("          \"columns\": [ \"TimeKeeper\",\"TimeKeeperName\" ], ");
    //  JsonTimeKeeperTest.Append("          \"rows\": ");
    //  JsonTimeKeeperTest.Append("          [ ");
    //  JsonTimeKeeperTest.Append("            [ \"LarryAxon\",\"Axon, Larry\" ] ");
    //  JsonTimeKeeperTest.Append("          ] ");
    //  JsonTimeKeeperTest.Append("        } ");
    //  JsonTimeKeeperTest.Append("      },");

    //  JsonContactTest.Append("      \"contact\": ");
    //  JsonContactTest.Append("      { ");
    //  JsonContactTest.Append("        \"type\": \"PickList\", ");
    //  JsonContactTest.Append("        \"value\": \"table\": ");
    //  JsonContactTest.Append("        { ");
    //  JsonContactTest.Append("          \"columns\": [ ], ");
    //  JsonContactTest.Append("          \"rows\": [ ] ");
    //  JsonContactTest.Append("        } ");
    //  JsonContactTest.Append("      },");

    //  JsonMatterTest.Append("      \"matter\": ");
    //  JsonMatterTest.Append("      { ");
    //  JsonMatterTest.Append("        \"type\": \"PickList\", ");
    //  JsonMatterTest.Append("        \"value\": \"table\": ");
    //  JsonMatterTest.Append("        { ");
    //  JsonMatterTest.Append("          \"columns\": [ \"Matter\",\"Description\" ], ");
    //  JsonMatterTest.Append("          \"rows\": ");
    //  JsonMatterTest.Append("          [ ");
    //  JsonMatterTest.Append("            [ \"LxT1M1\",\"Laxon Test1 Matter1\" ],");
    //  JsonMatterTest.Append("            [ \"LxT1M2\",\"Laxon Test1 Matter2\" ],");
    //  JsonMatterTest.Append("            [ \"LxT1M3\",\"Laxon Test1 Matter3\" ] ");
    //  JsonMatterTest.Append("          ] ");
    //  JsonMatterTest.Append("        } ");
    //  JsonMatterTest.Append("      },");

    //  JsonClientTest.Append("      \"client\": ");
    //  JsonClientTest.Append("      { ");
    //  JsonClientTest.Append("        \"type\": \"PickList\", ");
    //  JsonClientTest.Append("        \"value\": \"table\": ");
    //  JsonClientTest.Append("        { ");
    //  JsonClientTest.Append("          \"columns\": [ \"Client\",\"Name\" ], ");
    //  JsonClientTest.Append("          \"rows\": ");
    //  JsonClientTest.Append("          [ ");
    //  JsonClientTest.Append("            [ \"LaxonClient1\",\"LaxonTest1\" ],");
    //  JsonClientTest.Append("            [ \"LaxonClient2\",\"LaxonTest2\" ],");
    //  JsonClientTest.Append("            [ \"LaxonClient3\",\"LaxonTest3\" ],");
    //  JsonClientTest.Append("            [ \"LaxonClient4\",\"LaxonTest4\" ] ");
    //  JsonClientTest.Append("          ] ");
    //  JsonClientTest.Append("        } ");
    //  JsonClientTest.Append("      },");

    //  JsonAccountTest.Append("      \"account\": ");
    //  JsonAccountTest.Append("      { ");
    //  JsonAccountTest.Append("        \"type\": \"PickList\", ");
    //  JsonAccountTest.Append("        \"value\": \"table\": ");
    //  JsonAccountTest.Append("        { ");
    //  JsonAccountTest.Append("          \"columns\": [ \"Account\",\"AccountName\" ], ");
    //  JsonAccountTest.Append("          \"rows\": ");
    //  JsonAccountTest.Append("          [ ");
    //  JsonAccountTest.Append("            [ \"LaxonTestAccount\",\"Larry Axon, Test Account\" ],");
    //  JsonAccountTest.Append("            [ \"LegalEagles\",\"Varsey, Miller and PIlton\" ],");
    //  JsonAccountTest.Append("            [ \"SmithJones\",\"Smith, Jones, Farfield, and Wilson\" ] ");
    //  JsonAccountTest.Append("          ] ");
    //  JsonAccountTest.Append("        } ");
    //  JsonAccountTest.Append("      } ");

    //  JsonFormTest.Append(JsonTimeKeeperTest.ToString());
    //  JsonFormTest.Append(JsonContactTest.ToString());
    //  JsonFormTest.Append(JsonMatterTest.ToString());
    //  JsonFormTest.Append(JsonClientTest.ToString());
    //  JsonFormTest.Append(JsonAccountTest.ToString());
    //  JsonFormTest.Append("    }");

    //  JsonResultsTest.Append(JsonFormTest.ToString());
    //  JsonResultsTest.Append("  },");

    //  JsonSecurityContextTest.Append("  \"securitycontext\": ");
    //  JsonSecurityContextTest.Append("  { ");
    //  JsonSecurityContextTest.Append("    \"securityid\": \"110\", ");
    //  JsonSecurityContextTest.Append("    \"account\": \"LaxonTestAccount\", ");
    //  JsonSecurityContextTest.Append("    \"login\": \"laxon\", ");
    //  JsonSecurityContextTest.Append("    \"domain\": \"\", ");
    //  JsonSecurityContextTest.Append("    \"password\": \"\", \"security\":");
    //  JsonSecurityContextTest.Append("    {");
    //  JsonSecurityContextTest.Append("      \"user\":\"LarryAxon\",");
    //  JsonSecurityContextTest.Append("      \"isloggedIn\":True,");
    //  JsonSecurityContextTest.Append("      \"ParentGroups\":[\"timekeeper\"],");
    //  JsonSecurityContextTest.Append("      \"ObjectRightsList\":[{\"mainscreen\":\"Grant\"}],");
    //  JsonSecurityContextTest.Append("      \"ObjectTypes\":[\"mainscreen\":\"function\"]");
    //  JsonSecurityContextTest.Append("    } ");
    //  JsonSecurityContextTest.Append("  }, ");

    //  JsonTimeOptionsTest.Append("  \"timeoptions\": ");
    //  JsonTimeOptionsTest.Append("  { ");
    //  JsonTimeOptionsTest.Append("    \"parameters\": ");
    //  JsonTimeOptionsTest.Append("    { ");
    //  JsonTimeOptionsTest.Append("      \"useactivity\": \"true\",");
    //  JsonTimeOptionsTest.Append("      \"usetimer\": \"true\",");
    //  JsonTimeOptionsTest.Append("      \"initialscreen\": \"timeentry\",");
    //  JsonTimeOptionsTest.Append("      \"usephase\": \"true\",");
    //  JsonTimeOptionsTest.Append("      \"autostarttime\": \"true\",");
    //  JsonTimeOptionsTest.Append("      \"usetask\": \"true\",");
    //  JsonTimeOptionsTest.Append("      \"usedescription\": \"true\",");
    //  JsonTimeOptionsTest.Append("      \"codeset\": \"all\",");
    //  JsonTimeOptionsTest.Append("      \"useinternalnotes\": \"true\",");
    //  JsonTimeOptionsTest.Append("      \"timeincrement\": \"tenth\"");
    //  JsonTimeOptionsTest.Append("    } ");
    //  JsonTimeOptionsTest.Append("  }, ");

    //  JsonErrorsTest.Append("  \"errors\": [  ] ");

    //  JsonResultsTest.Append(JsonSecurityContextTest.ToString());
    //  JsonResultsTest.Append(JsonTimeOptionsTest.ToString());
    //  JsonResultsTest.Append(JsonErrorsTest.ToString());
    //  JsonResultsTest.Append("}");
    }

    [Test]
    public void stripDelimsSimpleTests()
    {
      //String JsonTestString = JsonResultsTest.ToString();
      //String JsonTestExpected = JsonTestString.Substring(1, JsonTestString.Length - 2).Trim();
      //String JsonTestProcessed = CommonFunctions.stripDelims(JsonTestString, CommonData.cLEFTCURLY);

      string seedString01 = "{curly}[square](parens)\"doublequote\"";
      string curlyExpected = "curly";
      string squareExpected = "square";
      string parensExpected = "parens";
      string doublequoteExpected = "doublequote";

      Assert.AreEqual(curlyExpected, CommonFunctions.stripDelims(seedString01, CommonData.cLEFTCURLY));
      Assert.AreEqual(squareExpected, CommonFunctions.stripDelims(seedString01, CommonData.cLEFTSQUARE));
      Assert.AreEqual(parensExpected, CommonFunctions.stripDelims(seedString01, CommonData.cLEFT));
      Assert.AreEqual(doublequoteExpected, CommonFunctions.stripDelims(seedString01, '"'));

      string seedString02 = "{curly[square(parens\"doublequote";
      curlyExpected = "curly[square(parens\"doublequote";
      squareExpected = "square(parens\"doublequote";
      parensExpected = "parens\"doublequote";
      doublequoteExpected = "doublequote";

      Assert.AreEqual(curlyExpected, CommonFunctions.stripDelims(seedString02, CommonData.cLEFTCURLY));
      Assert.AreEqual(squareExpected, CommonFunctions.stripDelims(seedString02, CommonData.cLEFTSQUARE));
      Assert.AreEqual(parensExpected, CommonFunctions.stripDelims(seedString02, CommonData.cLEFT));
      Assert.AreEqual(doublequoteExpected, CommonFunctions.stripDelims(seedString02, '"'));

      string seedString03 = "{curly[square(parens\"doublequote\")]}";
      curlyExpected = "curly[square(parens\"doublequote\")]";
      squareExpected = "square(parens\"doublequote\")";
      parensExpected = "parens\"doublequote\"";
      doublequoteExpected = "doublequote";

      Assert.AreEqual(curlyExpected, CommonFunctions.stripDelims(seedString03, CommonData.cLEFTCURLY));
      Assert.AreEqual(squareExpected, CommonFunctions.stripDelims(seedString03, CommonData.cLEFTSQUARE));
      Assert.AreEqual(parensExpected, CommonFunctions.stripDelims(seedString03, CommonData.cLEFT));
      Assert.AreEqual(doublequoteExpected, CommonFunctions.stripDelims(seedString03, '"'));

      string seedBadString01 = "}curly]square(parens\"doublequote\")[badsquare{badcurly";
      curlyExpected = "badcurly";
      squareExpected = "badsquare{badcurly";
      parensExpected = "parens\"doublequote\"";
      doublequoteExpected = "doublequote";

      Assert.AreEqual(curlyExpected, CommonFunctions.stripDelims(seedBadString01, CommonData.cLEFTCURLY));
      Assert.AreEqual(squareExpected, CommonFunctions.stripDelims(seedBadString01, CommonData.cLEFTSQUARE));
      Assert.AreEqual(parensExpected, CommonFunctions.stripDelims(seedBadString01, CommonData.cLEFT));
      Assert.AreEqual(doublequoteExpected, CommonFunctions.stripDelims(seedBadString01, '"'));

    }

    [Test]
    public void getDelimTokenSimpleTests()
    {
      string tokenList01 = "{curlyToken0}[squareToken0](parensToken0)(parensToken1)\"doublequoteToken0\"{curlyToken1}\"doublequoteToken1\"[squareToken1]";
      string curlyTokenExpected = "curlyToken0";
      string squareTokenExpected = "squareToken0";
      string parensTokenExpected = "parensToken0";
      string doublequoteTokenExpected = "doublequoteToken0";

      Assert.AreEqual(curlyTokenExpected, CommonFunctions.getDelimToken(tokenList01, CommonData.cLEFTCURLY, 1));
      Assert.AreEqual(squareTokenExpected, CommonFunctions.getDelimToken(tokenList01, CommonData.cLEFTSQUARE, 1));
      Assert.AreEqual(parensTokenExpected, CommonFunctions.getDelimToken(tokenList01, CommonData.cLEFT, 1));
      Assert.AreEqual(doublequoteTokenExpected, CommonFunctions.getDelimToken(tokenList01, '"', 1));

      curlyTokenExpected = "curlyToken1";
      squareTokenExpected = "squareToken1";
      parensTokenExpected = "parensToken1";
      doublequoteTokenExpected = "doublequoteToken1";

      Assert.AreEqual(curlyTokenExpected, CommonFunctions.getDelimToken(tokenList01, CommonData.cLEFTCURLY, 2));
      Assert.AreEqual(squareTokenExpected, CommonFunctions.getDelimToken(tokenList01, CommonData.cLEFTSQUARE, 2));
      Assert.AreEqual(parensTokenExpected, CommonFunctions.getDelimToken(tokenList01, CommonData.cLEFT, 2));
      Assert.AreEqual(doublequoteTokenExpected, CommonFunctions.getDelimToken(tokenList01, '"', 2));
    }

    [Test]
    public void properCaseTests()
    {
      string properCaseTest10 = "lUis mario vAllejo BACA!!";
      string properCaseExpected10 = " Luis Mario Vallejo Baca!!";

      string properCaseTest12 = "  luis mario   vAllejo BACA***!!";
      string properCaseExpected12 = "   Luis Mario   Vallejo Baca***!!";

      string properCaseTest20 = "{LUIS mARIO vAllejo BACA}";
      string properCaseExpected20 = " {LUIS mARIO vAllejo BACA}";

      string properCaseTest30 = "{[lUis mario vAllejo BACa";
      string properCaseExpected30 = " {[lUis mario vAllejo BACa";

      string properCaseTest40 = "LUIS (mario) vallejo *BACA*";
      string properCaseExpected40 = " Luis (mario) Vallejo *baca*";

      string properCaseTest50 = "\"lUis mario vAllejo BACA\"!";
      string properCaseExpected50 = " \"lUis mario vAllejo BACA\"!";

      string properCaseTest60 = "{lUis mario vAllejo BACA}!";
      string properCaseExpected60 = " {lUis mario vAllejo BACA}!";

      string properCaseTest70 = "lUis \"mario vAllejo \"BACA!";
      string properCaseExpected70 = " Luis \"mario vAllejo \"baca!";

      string properCaseTest80 = "lUis \"mario vAllejo \" BACA!";
      string properCaseExpected80 = " Luis \"mario vAllejo \" Baca!";


      Assert.AreEqual(properCaseExpected10, CommonFunctions.properCase(properCaseTest10));
      Assert.AreEqual(properCaseExpected12, CommonFunctions.properCase(properCaseTest12));
      Assert.AreEqual(properCaseExpected20, CommonFunctions.properCase(properCaseTest20));
      Assert.AreEqual(properCaseExpected30, CommonFunctions.properCase(properCaseTest30));
      Assert.AreEqual(properCaseExpected40, CommonFunctions.properCase(properCaseTest40));
      Assert.AreEqual(properCaseExpected50, CommonFunctions.properCase(properCaseTest50));
      Assert.AreEqual(properCaseExpected60, CommonFunctions.properCase(properCaseTest60));
      Assert.AreEqual(properCaseExpected70, CommonFunctions.properCase(properCaseTest70));
      Assert.AreEqual(properCaseExpected80, CommonFunctions.properCase(properCaseTest80));
    }

    [Test]
    public void parseStringTests()
    {
      StringBuilder sb_parseTestString01 = new StringBuilder();
      sb_parseTestString01.Append("\"account\": { \"type\": \"PickList\", \"value\": { \"table\": { \"columns\": [ \"Account\", \"AccountName\" ], ");
      sb_parseTestString01.Append("\"rows\": [ [ \"LaxonTestAccount\", \"Larry Axon, Test Account\" ], [ \"LegalEagles\", \"Varsey, Miller and PIlton\" ], ");
      sb_parseTestString01.Append("[ \"SmithJones\", \"Smith, Jones, Farfield, and Wilson\" ] ] } } }");

      string parseStringTest01 = sb_parseTestString01.ToString();
      string parseStringTest02 = CommonFunctions.stripDelims(parseStringTest01, CommonData.cLEFTCURLY);

      string[] parsePickListTable = CommonFunctions.parseString(parseStringTest02, new string[] { "," });

      string key = parsePickListTable[0];
      string val = parsePickListTable[1];

      string tableJson = CommonFunctions.stripDelims(val, CommonData.cLEFTCURLY);

      string[] tableJsonParts = CommonFunctions.parseString(tableJson, new string[] { ":" });

      key = tableJsonParts[0];
      val = tableJsonParts[1];

      //parsePickListTable = CommonFunctions.parseString(val, new string[] { ":" });

      //CCI.Common.CCITable ATKTable = new CCI.Common.CCITable();

      //CCITable = CCI.Common.ServerResponseSerializer.JsonToTable(val);

    }

    #endregion parsing function tests!

    #region CDate conversion tests!

    [Test]
    public void CDateSimpleTests()
    { 
      /*
       * TODO: We have a problem with regionalization, dates are not being converted correctly, I have my region in Honduras, and will try it now
       * based on the little first research we did with Larry, we have to find a kind of "independent" way of solving this, and have a "universal" 
       * date format, so as for this to work correctly
       */
      /*
       * This are results of the immediate window tests we did with Larry:
          ? o
          "2010-06-30 14:30:15:000"
          o = "2010/06/30 14:30:15:000"
          "2010/06/30 14:30:15:000"
          o = "2010/30/06 14:30:15:000"
          "2010/30/06 14:30:15:000"
          o = "2010/30/06 02:30:15:000 pm"
          "2010/30/06 02:30:15:000 pm"
          o = "2010-30-06 14:30:15:000"
          "2010-30-06 14:30:15:000"
          ?DateTime.Now
          {03/09/2012 11:33:29 a.m.}
              Date: {03/09/2012 12:00:00 a.m.}
              Day: 3
              DayOfWeek: Monday
              DayOfYear: 247
              Hour: 11
              Kind: Local
              Millisecond: 275
              Minute: 33
              Month: 9
              Second: 29
              Ticks: 634822688092753906
              TimeOfDay: {11:33:29.2753906}
              Year: 2012
          o = "30-09-2012 02:30:15:000 p.m."
          "30-09-2012 02:30:15:000 p.m."
          o = "30/09/2012 02:30:15:000 p.m."
          "30/09/2012 02:30:15:000 p.m."
          o = "30/09/2012 02:30:15 p.m."
          "30/09/2012 02:30:15 p.m."
          o = "09/30/2012 02:30:15 p.m."
          "09/30/2012 02:30:15 p.m."
      */

      //We begin with some "ramdom" dates I just come up for the createTimeRecord and updateTimeRecord NUNIT tests

      //Create a NUNIT for CdateTime using this time format o = "9/14/2012 2:22:35 PM"

      DateTime startDateTime = new DateTime(2010, 06, 30, 14, 30, 15);
      DateTime endDateTime = new DateTime(2012, 10, 20, 22, 55, 00);
      DateTime justDate = new DateTime(2012, 10, 30);

      DateTime lastModifiedDateTime = DateTime.Now;

      string DateExpected = startDateTime.ToString(CommonData.FORMATLONGDATETIME);
      string DateObtained = CommonFunctions.CDateTime(DateExpected).ToString(CommonData.FORMATLONGDATETIME);

      Assert.AreEqual(DateExpected, DateObtained);

      DateExpected = endDateTime.ToString(CommonData.FORMATLONGDATETIME);
      DateObtained = CommonFunctions.CDateTime(DateExpected).ToString(CommonData.FORMATLONGDATETIME);

      Assert.AreEqual(DateExpected, DateObtained);

      DateExpected = justDate.ToString(CommonData.FORMATSHORTDATE);
      DateObtained = CommonFunctions.CDateTime(DateExpected).ToString(CommonData.FORMATSHORTDATE);

      Assert.AreEqual(DateExpected, DateObtained);

      DateExpected = "2012-09-14";
      DateObtained = CommonFunctions.CDateTime("9/14/2012 2:22:35 PM").ToString(CommonData.FORMATSHORTDATE);

      Assert.AreEqual(DateExpected, DateObtained);

      DateExpected = "2012-06-07 14:22:35:000";
      DateObtained = CommonFunctions.CDateTime("06/07/2012 2:22:35 PM").ToString(CommonData.FORMATLONGDATETIME);

      Assert.AreEqual(DateExpected, DateObtained);

    }

    #endregion CDate conversion tests!

    #region CString conversion tests!

    [Test]
    public void CStringSimpleTests()
    {
      /*
       * TODO: We have a problem with regionalization, dates are not being converted correctly, I have my region in Honduras, and will try it now
       * based on the little first research we did with Larry, we have to find a kind of "independent" way of solving this, and have a "universal" 
       * date format, so as for this to work correctly
       */

      //We begin with some "ramdom" dates I just come up for the createTimeRecord and updateTimeRecord NUNIT tests
      DateTime startDateTime = new DateTime(2010, 06, 30, 14, 30, 15);
      DateTime endDateTime = new DateTime(2012, 10, 20, 22, 55, 00);
      DateTime justDate = new DateTime(2012, 10, 30);

      DateTime lastModifiedDateTime = DateTime.Now;

      //string DateExpected = startDateTime.ToString(CommonData.FORMATLONGDATETIME);
      //string DateObtained = CommonFunctions.CDateTime(DateExpected).ToString(CommonData.FORMATLONGDATETIME);

      //Assert.AreEqual(DateExpected, DateObtained);

      //DateExpected = endDateTime.ToString(CommonData.FORMATLONGDATETIME);
      //DateObtained = CommonFunctions.CDateTime(DateExpected).ToString(CommonData.FORMATLONGDATETIME);

      //Assert.AreEqual(DateExpected, DateObtained);

      string DateExpected = justDate.ToString(CommonData.FORMATSHORTDATE);
      string DateObtained = CommonFunctions.CString(justDate);

      Assert.AreEqual(DateExpected, DateObtained);

      DateExpected = startDateTime.ToString(CommonData.FORMATLONGDATETIME);
      DateObtained = CommonFunctions.CString(startDateTime);

      Assert.AreEqual(DateExpected, DateObtained);
    }

    #endregion CString conversion tests!

    #region Encrypt/Decrypt conversion tests!

    [Test]
    public void EncryptDecryptSimpleTests()
    {
      /*
       * TODO: We have a problem with regionalization, dates are not being converted correctly, I have my region in Honduras, and will try it now
       * based on the little first research we did with Larry, we have to find a kind of "independent" way of solving this, and have a "universal" 
       * date format, so as for this to work correctly
       */
      EncryptDecryptString edServer = new EncryptDecryptString();

      string loginSeed = "laxon";
      string loginEncrypted = string.Empty;
      string loginDecrypted = "laxon";
      
      loginEncrypted = edServer.encryptString(loginSeed);
      loginDecrypted = edServer.decryptString(loginEncrypted);

      Assert.AreNotEqual(loginSeed, loginEncrypted);
      Assert.AreEqual(loginSeed, loginDecrypted);

      //12.45
      loginSeed = "12.45";
      loginEncrypted = string.Empty;
      loginDecrypted = "12.45";

      loginEncrypted = edServer.encryptString(loginSeed);
      loginDecrypted = edServer.decryptString(loginEncrypted);

      Assert.AreNotEqual(loginSeed, loginEncrypted);
      Assert.AreEqual(loginSeed, loginDecrypted);

      loginSeed = "lmv66chess";
      loginEncrypted = string.Empty;
      loginDecrypted = "lmv66chess";

      loginEncrypted = edServer.encryptString(loginSeed);
      loginDecrypted = edServer.decryptString(loginEncrypted);

      Assert.AreNotEqual(loginSeed, loginEncrypted);
      Assert.AreEqual(loginSeed, loginDecrypted);

      loginSeed = "lmv66_New!!";
      loginEncrypted = string.Empty;
      loginDecrypted = "lmv66_New!!";

      loginEncrypted = edServer.encryptString(loginSeed);
      loginDecrypted = edServer.decryptString(loginEncrypted);

      Assert.AreNotEqual(loginSeed, loginEncrypted);
      Assert.AreEqual(loginSeed, loginDecrypted);

    }

    #endregion Encrypt/Decrypt conversion tests!


  }
}
