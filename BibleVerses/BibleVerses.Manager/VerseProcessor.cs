﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

using BibleVerses.Models;
using BibleVerses.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BibleVerses.Manager
{
  public class VerseProcessor : IDisposable
  {
    public void Dispose() { }
    #region verses
    public IEnumerable<BibleVerseModel> GetBibleVerses(string username, string group)
    {
      return DataSource.GetBibleVerses(username, group);
    }
    public BibleVerseModel GetNextVerse(string username, string group)
    {
      BibleVerseModel verse = null;
      int lastVerseID = DataSource.GetLastVerseDisplayed(username, group);
      IEnumerable<BibleVerseModel> verses = DataSource.GetBibleVerses(username, group);
      if (verses != null && verses.Count() > 0)
      {
        // now fine the next verse
        if (lastVerseID == -1 || lastVerseID == verses.Count() ) // no last verse or the last one was the last verse in the sequence
          verse = verses.FirstOrDefault(); //return the first one.
        else
        {
          bool versefound = false;
          foreach (BibleVerseModel v in verses)
          {
            if (versefound)
            {
              verse = v;
              break;
            }
            if (v.ID == lastVerseID)
              versefound = true;
          }
          if ((versefound && verse == null)  ||// last verse is the LAST verse :-)
           (verse == null && verses.Count() > 0))
            verse = verses.FirstOrDefault();
        }
      }
      if (verse != null)
        DataSource.updateLastVerseDisplayed(username, group, verse.ID);
      else
        verse = new BibleVerseModel();
      return verse;
    }
    public int SaveVerse(BibleVerseModel verse)
    {
      return DataSource.SaveVerse(verse);
    }
    public void DeleteVerse(int id)
    {
      DataSource.DeleteVerse(id);
    }
    #endregion
    #region bible api
    public Dictionary<string, string> GetBibleList(string language)
    {
      Dictionary<string, string> returnList = new Dictionary<string, string>();
      dynamic bibles = BibleAPI.GetBibles(language);
      for (int i=0; i < bibles.data.Count; i++)
      {
        JObject bible = bibles.data[i];
        string abbreviation = bible["abbreviation"].ToString();
        string name = bible["name"].ToString();
        if (returnList.ContainsKey(abbreviation))
          returnList[abbreviation] = name;
        else
          returnList.Add(abbreviation, name);

      }
      return returnList;
      /*
       *{
  "data": [
    {
      "id": "6bab4d6c61b31b80-01",
      "dblId": "6bab4d6c61b31b80",
      "relatedDbl": null,
      "name": "Brenton English Septuagint (Updated Spelling and Formatting)",
      "nameLocal": "Brenton English Septuagint (Updated Spelling and Formatting)",
      "abbreviation": "engLXXup",
      "abbreviationLocal": "LXXup",
      "description": "common",
      "descriptionLocal": "common",
      "language": {
        "id": "eng",
        "name": "English",
        "nameLocal": "English",
        "script": "Latin",
        "scriptDirection": "LTR"
      },
      "countries": [
        {
          "id": "ZZ",
          "name": "Unspecific",
          "nameLocal": "Unspecific"
        }
      ],
      "type": "text",
      "updatedAt": "2020-07-28T15:54:25.000Z",
      "audioBibles": []
    },
    {
      "id": "65bfdebd704a8324-01",
      "dblId": "65bfdebd704a8324",
      "relatedDbl": null,
      "name": "Brenton English translation of the Septuagint",
      "nameLocal": "Brenton English translation of the Septuagint",
      "abbreviation": "engbrent",
      "abbreviationLocal": "Brenton",
      "description": "Septuagint",
      "descriptionLocal": "Septuagint",
      "language": {
        "id": "eng",
        "name": "English",
        "nameLocal": "English",
        "script": "Latin",
        "scriptDirection": "LTR"
      },
      "countries": [
        {
          "id": "GB",
          "name": "United Kingdom",
          "nameLocal": "United Kingdom"
        },
        {
          "id": "US",
          "name": "United States",
          "nameLocal": "United States"
        }
      ],
      "type": "text",
      "updatedAt": "2020-09-11T20:50:20.000Z",
      "audioBibles": []
    },
    {
      "id": "55212e3cf5d04d49-01",
      "dblId": "55212e3cf5d04d49",
      "relatedDbl": null,
      "name": "Cambridge Paragraph Bible of the KJV",
      "nameLocal": "Cambridge Paragraph Bible of the KJV",
      "abbreviation": "engKJVCPB",
      "abbreviationLocal": "KJVCPB",
      "description": "Common",
      "descriptionLocal": "Common",
      "language": {
        "id": "eng",
        "name": "English",
        "nameLocal": "English",
        "script": "Latin",
        "scriptDirection": "LTR"
      },
      "countries": [
        {
          "id": "US",
          "name": "United States",
          "nameLocal": "United States"
        }
      ],
      "type": "text",
      "updatedAt": "2020-09-09T21:52:35.000Z",
      "audioBibles": []
    },
    {
      "id": "179568874c45066f-01",
      "dblId": "179568874c45066f",
      "relatedDbl": null,
      "name": "Douay-Rheims American 1899",
      "nameLocal": "Douay-Rheims American 1899",
      "abbreviation": "engDRA",
      "abbreviationLocal": "DRA",
      "description": "The Holy Bible in English, Douay-Rheims American Edition of 1899, translated from the Latin Vulgate",
      "descriptionLocal": "Catholic",
      "language": {
        "id": "eng",
        "name": "English",
        "nameLocal": "English",
        "script": "Latin",
        "scriptDirection": "LTR"
      },
      "countries": [
        {
          "id": "US",
          "name": "United States",
          "nameLocal": "United States"
        }
      ],
      "type": "text",
      "updatedAt": "2020-10-12T20:18:43.000Z",
      "audioBibles": []
    },
    {
      "id": "55ec700d9e0d77ea-01",
      "dblId": "55ec700d9e0d77ea",
      "relatedDbl": null,
      "name": "English Majority Text Version",
      "nameLocal": "English Majority Text Version",
      "abbreviation": "engEMTV",
      "abbreviationLocal": "EMTV",
      "description": "common",
      "descriptionLocal": "common",
      "language": {
        "id": "eng",
        "name": "English",
        "nameLocal": "English",
        "script": "Latin",
        "scriptDirection": "LTR"
      },
      "countries": [
        {
          "id": "US",
          "name": "United States",
          "nameLocal": "United States"
        }
      ],
      "type": "text",
      "updatedAt": "2020-09-11T20:03:01.000Z",
      "audioBibles": []
    },
    {
      "id": "65eec8e0b60e656b-01",
      "dblId": "65eec8e0b60e656b",
      "relatedDbl": null,
      "name": "Free Bible Version",
      "nameLocal": "Free Bible Version",
      "abbreviation": "FBV",
      "abbreviationLocal": "FBV",
      "description": "Protestant FBV full 3.0 beta",
      "descriptionLocal": "Protestant FBV full 3.0 beta",
      "language": {
        "id": "eng",
        "name": "English",
        "nameLocal": "English",
        "script": "Latin",
        "scriptDirection": "LTR"
      },
      "countries": [
        {
          "id": "US",
          "name": "United States",
          "nameLocal": "United States"
        }
      ],
      "type": "text",
      "updatedAt": "2020-09-11T20:51:13.000Z",
      "audioBibles": []
    },
    {
      "id": "c315fa9f71d4af3a-01",
      "dblId": "c315fa9f71d4af3a",
      "relatedDbl": null,
      "name": "Geneva Bible",
      "nameLocal": "Geneva Bible",
      "abbreviation": "enggnv",
      "abbreviationLocal": "GNV",
      "description": "common",
      "descriptionLocal": "common",
      "language": {
        "id": "eng",
        "name": "English",
        "nameLocal": "English",
        "script": "Latin",
        "scriptDirection": "LTR"
      },
      "countries": [
        {
          "id": "GB",
          "name": "United Kingdom",
          "nameLocal": "United Kingdom"
        }
      ],
      "type": "text",
      "updatedAt": "2020-09-15T05:45:48.000Z",
      "audioBibles": []
    },
    {
      "id": "bf8f1c7f3f9045a5-01",
      "dblId": "bf8f1c7f3f9045a5",
      "relatedDbl": null,
      "name": "JPS TaNaKH 1917",
      "nameLocal": "JPS TaNaKH 1917",
      "abbreviation": "engojps",
      "abbreviationLocal": "OJPS",
      "description": "Jewish",
      "descriptionLocal": "Jewish",
      "language": {
        "id": "eng",
        "name": "English",
        "nameLocal": "English",
        "script": "Latin",
        "scriptDirection": "LTR"
      },
      "countries": [
        {
          "id": "US",
          "name": "United States",
          "nameLocal": "United States"
        }
      ],
      "type": "text",
      "updatedAt": "2020-07-28T23:12:45.000Z",
      "audioBibles": []
    },
    {
      "id": "de4e12af7f28f599-01",
      "dblId": "de4e12af7f28f599",
      "relatedDbl": null,
      "name": "King James (Authorised) Version",
      "nameLocal": "King James Version",
      "abbreviation": "engKJV",
      "abbreviationLocal": "KJV",
      "description": "Ecumenical",
      "descriptionLocal": "Ecumenical",
      "language": {
        "id": "eng",
        "name": "English",
        "nameLocal": "English",
        "script": "Latin",
        "scriptDirection": "LTR"
      },
      "countries": [
        {
          "id": "GB",
          "name": "United Kingdom",
          "nameLocal": "United Kingdom"
        }
      ],
      "type": "text",
      "updatedAt": "2020-07-29T01:28:14.000Z",
      "audioBibles": []
    },
    {
      "id": "de4e12af7f28f599-02",
      "dblId": "de4e12af7f28f599",
      "relatedDbl": null,
      "name": "King James (Authorised) Version",
      "nameLocal": "King James Version",
      "abbreviation": "engKJV",
      "abbreviationLocal": "KJV",
      "description": "Protestant",
      "descriptionLocal": "Protestant",
      "language": {
        "id": "eng",
        "name": "English",
        "nameLocal": "English",
        "script": "Latin",
        "scriptDirection": "LTR"
      },
      "countries": [
        {
          "id": "GB",
          "name": "United Kingdom",
          "nameLocal": "United Kingdom"
        }
      ],
      "type": "text",
      "updatedAt": "2020-07-29T01:29:10.000Z",
      "audioBibles": []
    },
    {
      "id": "01b29f4b342acc35-01",
      "dblId": "01b29f4b342acc35",
      "relatedDbl": null,
      "name": "Literal Standard Version",
      "nameLocal": "Literal Standard Version",
      "abbreviation": "LSV",
      "abbreviationLocal": "LSV",
      "description": "Protestant",
      "descriptionLocal": "Protestant",
      "language": {
        "id": "eng",
        "name": "English",
        "nameLocal": "English",
        "script": "Latin",
        "scriptDirection": "LTR"
      },
      "countries": [
        {
          "id": "US",
          "name": "United States",
          "nameLocal": "United States"
        },
        {
          "id": "GB",
          "name": "United Kingdom",
          "nameLocal": "United Kingdom"
        },
        {
          "id": "CA",
          "name": "Canada",
          "nameLocal": "Canada"
        },
        {
          "id": "AU",
          "name": "Australia",
          "nameLocal": "Australia"
        },
        {
          "id": "ZA",
          "name": "South Africa",
          "nameLocal": "South Africa"
        },
        {
          "id": "NZ",
          "name": "New Zealand",
          "nameLocal": "New Zealand"
        }
      ],
      "type": "text",
      "updatedAt": "2020-10-14T09:07:54.000Z",
      "audioBibles": []
    },
    {
      "id": "40072c4a5aba4022-01",
      "dblId": "40072c4a5aba4022",
      "relatedDbl": null,
      "name": "Revised Version 1885",
      "nameLocal": "Revised Version 1885",
      "abbreviation": "engRV",
      "abbreviationLocal": "RV",
      "description": "Interconfessional",
      "descriptionLocal": "Interconfessional",
      "language": {
        "id": "eng",
        "name": "English",
        "nameLocal": "English",
        "script": "Latin",
        "scriptDirection": "LTR"
      },
      "countries": [
        {
          "id": "GB",
          "name": "United Kingdom",
          "nameLocal": "United Kingdom"
        }
      ],
      "type": "text",
      "updatedAt": "2020-09-10T20:48:17.000Z",
      "audioBibles": []
    },
    {
      "id": "ec290b5045ff54a5-01",
      "dblId": "ec290b5045ff54a5",
      "relatedDbl": null,
      "name": "Targum Onkelos Etheridge",
      "nameLocal": "Targum Onkelos Etheridge",
      "abbreviation": "engOKE",
      "abbreviationLocal": "OKE",
      "description": "common",
      "descriptionLocal": "common",
      "language": {
        "id": "eng",
        "name": "English",
        "nameLocal": "English",
        "script": "Latin",
        "scriptDirection": "LTR"
      },
      "countries": [
        {
          "id": "ZZ",
          "name": "Unspecific",
          "nameLocal": "Unspecific"
        }
      ],
      "type": "text",
      "updatedAt": "2020-07-29T02:31:40.000Z",
      "audioBibles": []
    },
    {
      "id": "2f0fd81d7b85b923-01",
      "dblId": "2f0fd81d7b85b923",
      "relatedDbl": null,
      "name": "The English New Testament According to Family 35",
      "nameLocal": "The English New Testament According to Family 35",
      "abbreviation": "engF35",
      "abbreviationLocal": "F35",
      "description": "Common",
      "descriptionLocal": "Common",
      "language": {
        "id": "eng",
        "name": "English",
        "nameLocal": "English",
        "script": "Latin",
        "scriptDirection": "LTR"
      },
      "countries": [
        {
          "id": "ZZ",
          "name": "Unspecific",
          "nameLocal": "Unspecific"
        }
      ],
      "type": "text",
      "updatedAt": "2020-09-09T21:52:33.000Z",
      "audioBibles": []
    },
    {
      "id": "06125adad2d5898a-01",
      "dblId": "06125adad2d5898a",
      "relatedDbl": null,
      "name": "The Holy Bible, American Standard Version",
      "nameLocal": "The Holy Bible, American Standard Version",
      "abbreviation": "ASV",
      "abbreviationLocal": "ASV",
      "description": "Bible",
      "descriptionLocal": "Bible",
      "language": {
        "id": "eng",
        "name": "English",
        "nameLocal": "English",
        "script": "Latin",
        "scriptDirection": "LTR"
      },
      "countries": [
        {
          "id": "US",
          "name": "United States",
          "nameLocal": "United States"
        }
      ],
      "type": "text",
      "updatedAt": "2020-09-10T20:39:14.000Z",
      "audioBibles": []
    },
    {
      "id": "66c22495370cdfc0-01",
      "dblId": "66c22495370cdfc0",
      "relatedDbl": null,
      "name": "Translation for Translators",
      "nameLocal": "Translation for Translators",
      "abbreviation": "T4T",
      "abbreviationLocal": "T4T",
      "description": "common",
      "descriptionLocal": "common",
      "language": {
        "id": "eng",
        "name": "English",
        "nameLocal": "English",
        "script": "Latin",
        "scriptDirection": "LTR"
      },
      "countries": [
        {
          "id": "US",
          "name": "United States",
          "nameLocal": "United States"
        }
      ],
      "type": "text",
      "updatedAt": "2020-09-11T20:54:08.000Z",
      "audioBibles": []
    },
    {
      "id": "9879dbb7cfe39e4d-01",
      "dblId": "9879dbb7cfe39e4d",
      "relatedDbl": null,
      "name": "World English Bible",
      "nameLocal": "World English Bible",
      "abbreviation": "WEB",
      "abbreviationLocal": "WEB",
      "description": "Ecumenical",
      "descriptionLocal": "Ecumenical",
      "language": {
        "id": "eng",
        "name": "English",
        "nameLocal": "English",
        "script": "Latin",
        "scriptDirection": "LTR"
      },
      "countries": [
        {
          "id": "US",
          "name": "United States",
          "nameLocal": "United States"
        }
      ],
      "type": "text",
      "updatedAt": "2020-10-10T09:12:36.000Z",
      "audioBibles": [
        {
          "id": "105a06b6146d11e7-01",
          "name": "English - World English Bible (NT)",
          "nameLocal": "English - World English Bible (NT)",
          "dblId": "105a06b6146d11e7"
        }
      ]
    },
    {
      "id": "9879dbb7cfe39e4d-02",
      "dblId": "9879dbb7cfe39e4d",
      "relatedDbl": null,
      "name": "World English Bible",
      "nameLocal": "World English Bible",
      "abbreviation": "WEB",
      "abbreviationLocal": "WEB",
      "description": "Catholic",
      "descriptionLocal": "Catholic",
      "language": {
        "id": "eng",
        "name": "English",
        "nameLocal": "English",
        "script": "Latin",
        "scriptDirection": "LTR"
      },
      "countries": [
        {
          "id": "US",
          "name": "United States",
          "nameLocal": "United States"
        }
      ],
      "type": "text",
      "updatedAt": "2020-10-10T09:13:12.000Z",
      "audioBibles": [
        {
          "id": "105a06b6146d11e7-01",
          "name": "English - World English Bible (NT)",
          "nameLocal": "English - World English Bible (NT)",
          "dblId": "105a06b6146d11e7"
        }
      ]
    },
    {
      "id": "9879dbb7cfe39e4d-03",
      "dblId": "9879dbb7cfe39e4d",
      "relatedDbl": null,
      "name": "World English Bible",
      "nameLocal": "World English Bible",
      "abbreviation": "WEB",
      "abbreviationLocal": "WEB",
      "description": "Orthodox",
      "descriptionLocal": "Orthodox",
      "language": {
        "id": "eng",
        "name": "English",
        "nameLocal": "English",
        "script": "Latin",
        "scriptDirection": "LTR"
      },
      "countries": [
        {
          "id": "US",
          "name": "United States",
          "nameLocal": "United States"
        }
      ],
      "type": "text",
      "updatedAt": "2020-10-10T09:13:43.000Z",
      "audioBibles": [
        {
          "id": "105a06b6146d11e7-01",
          "name": "English - World English Bible (NT)",
          "nameLocal": "English - World English Bible (NT)",
          "dblId": "105a06b6146d11e7"
        }
      ]
    },
    {
      "id": "9879dbb7cfe39e4d-04",
      "dblId": "9879dbb7cfe39e4d",
      "relatedDbl": null,
      "name": "World English Bible",
      "nameLocal": "World English Bible",
      "abbreviation": "WEB",
      "abbreviationLocal": "WEB",
      "description": "Protestant",
      "descriptionLocal": "Protestant",
      "language": {
        "id": "eng",
        "name": "English",
        "nameLocal": "English",
        "script": "Latin",
        "scriptDirection": "LTR"
      },
      "countries": [
        {
          "id": "US",
          "name": "United States",
          "nameLocal": "United States"
        }
      ],
      "type": "text",
      "updatedAt": "2020-10-10T09:14:18.000Z",
      "audioBibles": [
        {
          "id": "105a06b6146d11e7-01",
          "name": "English - World English Bible (NT)",
          "nameLocal": "English - World English Bible (NT)",
          "dblId": "105a06b6146d11e7"
        }
      ]
    },
    {
      "id": "7142879509583d59-01",
      "dblId": "7142879509583d59",
      "relatedDbl": null,
      "name": "World English Bible British Edition",
      "nameLocal": "World English Bible British Edition",
      "abbreviation": "WEBBE",
      "abbreviationLocal": "WEBBE",
      "description": "Ecumenical",
      "descriptionLocal": "Ecumenical",
      "language": {
        "id": "eng",
        "name": "English",
        "nameLocal": "English",
        "script": "Latin",
        "scriptDirection": "LTR"
      },
      "countries": [
        {
          "id": "GB",
          "name": "United Kingdom",
          "nameLocal": "United Kingdom"
        }
      ],
      "type": "text",
      "updatedAt": "2020-10-15T09:10:03.000Z",
      "audioBibles": []
    },
    {
      "id": "7142879509583d59-02",
      "dblId": "7142879509583d59",
      "relatedDbl": null,
      "name": "World English Bible British Edition",
      "nameLocal": "World English Bible British Edition",
      "abbreviation": "WEBBE",
      "abbreviationLocal": "WEBBE",
      "description": "Catholic",
      "descriptionLocal": "Catholic",
      "language": {
        "id": "eng",
        "name": "English",
        "nameLocal": "English",
        "script": "Latin",
        "scriptDirection": "LTR"
      },
      "countries": [
        {
          "id": "GB",
          "name": "United Kingdom",
          "nameLocal": "United Kingdom"
        }
      ],
      "type": "text",
      "updatedAt": "2020-10-15T09:10:36.000Z",
      "audioBibles": []
    },
    {
      "id": "7142879509583d59-03",
      "dblId": "7142879509583d59",
      "relatedDbl": null,
      "name": "World English Bible British Edition",
      "nameLocal": "World English Bible British Edition",
      "abbreviation": "WEBBE",
      "abbreviationLocal": "WEBBE",
      "description": "Orthodox",
      "descriptionLocal": "Orthodox",
      "language": {
        "id": "eng",
        "name": "English",
        "nameLocal": "English",
        "script": "Latin",
        "scriptDirection": "LTR"
      },
      "countries": [
        {
          "id": "GB",
          "name": "United Kingdom",
          "nameLocal": "United Kingdom"
        }
      ],
      "type": "text",
      "updatedAt": "2020-10-15T09:11:09.000Z",
      "audioBibles": []
    },
    {
      "id": "7142879509583d59-04",
      "dblId": "7142879509583d59",
      "relatedDbl": null,
      "name": "World English Bible British Edition",
      "nameLocal": "World English Bible British Edition",
      "abbreviation": "WEBBE",
      "abbreviationLocal": "WEBBE",
      "description": "Protestant",
      "descriptionLocal": "Protestant",
      "language": {
        "id": "eng",
        "name": "English",
        "nameLocal": "English",
        "script": "Latin",
        "scriptDirection": "LTR"
      },
      "countries": [
        {
          "id": "GB",
          "name": "United Kingdom",
          "nameLocal": "United Kingdom"
        }
      ],
      "type": "text",
      "updatedAt": "2020-10-15T09:11:41.000Z",
      "audioBibles": []
    },
    {
      "id": "f72b840c855f362c-04",
      "dblId": "f72b840c855f362c",
      "relatedDbl": null,
      "name": "World Messianic Bible",
      "nameLocal": "World Messianic Bible",
      "abbreviation": "WMB",
      "abbreviationLocal": "WMB",
      "description": "Messianic",
      "descriptionLocal": "Messianic",
      "language": {
        "id": "eng",
        "name": "English",
        "nameLocal": "English",
        "script": "Latin",
        "scriptDirection": "LTR"
      },
      "countries": [
        {
          "id": "US",
          "name": "United States",
          "nameLocal": "United States"
        }
      ],
      "type": "text",
      "updatedAt": "2020-10-15T09:16:07.000Z",
      "audioBibles": []
    },
    {
      "id": "04da588535d2f823-04",
      "dblId": "04da588535d2f823",
      "relatedDbl": null,
      "name": "World Messianic Bible British Edition",
      "nameLocal": "World Messianic Bible British Edition",
      "abbreviation": "WMBBE",
      "abbreviationLocal": "WMBBE",
      "description": "Messianic",
      "descriptionLocal": "Messianic",
      "language": {
        "id": "eng",
        "name": "English",
        "nameLocal": "English",
        "script": "Latin",
        "scriptDirection": "LTR"
      },
      "countries": [
        {
          "id": "AU",
          "name": "Australia",
          "nameLocal": "Australia"
        },
        {
          "id": "AS",
          "name": "American Samoa",
          "nameLocal": "American Samoa"
        },
        {
          "id": "BS",
          "name": "Bahamas",
          "nameLocal": "Bahamas"
        },
        {
          "id": "BZ",
          "name": "Belize",
          "nameLocal": "Belize"
        },
        {
          "id": "IO",
          "name": "British Indian Ocean Territory",
          "nameLocal": "British Indian Ocean Territory"
        },
        {
          "id": "VG",
          "name": "Virgin Islands, British",
          "nameLocal": "Virgin Islands, British"
        },
        {
          "id": "CA",
          "name": "Canada",
          "nameLocal": "Canada"
        },
        {
          "id": "KE",
          "name": "Kenya",
          "nameLocal": "Kenya"
        },
        {
          "id": "FM",
          "name": "Micronesia, Federated States of",
          "nameLocal": "Micronesia, Federated States of"
        },
        {
          "id": "NZ",
          "name": "New Zealand",
          "nameLocal": "New Zealand"
        },
        {
          "id": "PG",
          "name": "Papua New Guinea",
          "nameLocal": "Papua New Guinea"
        },
        {
          "id": "WS",
          "name": "Samoa",
          "nameLocal": "Samoa"
        },
        {
          "id": "SG",
          "name": "Singapore",
          "nameLocal": "Singapore"
        },
        {
          "id": "TO",
          "name": "Tonga",
          "nameLocal": "Tonga"
        },
        {
          "id": "GB",
          "name": "United Kingdom",
          "nameLocal": "United Kingdom"
        }
      ],
      "type": "text",
      "updatedAt": "2020-10-15T09:07:59.000Z",
      "audioBibles": []
    }
  ]
}
      */
    }
    #endregion
  }
}
