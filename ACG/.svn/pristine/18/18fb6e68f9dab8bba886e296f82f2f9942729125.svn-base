using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;

using ACG.Common;
using ACG.Sys.Data;

namespace ACG.Sys.SecurityEngine
{
    public class SecurityGroups
    {
        private string m_GroupTableName;
        private SecurityDB mySecurityDB;
        private DataSet dsGroups = new DataSet();
        private DataSet dsAllGroups = new DataSet();
        private const string cMTYPEGROUP = "Group";
        private string tblGroups;
        private string tblObjectGroups;
        
        public SecurityGroups()
        {
            mySecurityDB = new SecurityDB();
            tblGroups = mySecurityDB.tblGroups;
            tblObjectGroups = mySecurityDB.tblObjectGroups;
        }
        
        public SecurityGroups(string whichTable)
        {
            mySecurityDB = new SecurityDB();
            tblGroups = mySecurityDB.tblGroups;
            tblObjectGroups = mySecurityDB.tblObjectGroups;
            m_GroupTableName = whichTable;
            Load();
        }
        
        public void Load()
        {
            if (m_GroupTableName == tblGroups)
            {
                dsAllGroups = mySecurityDB.getGroups();
            }
            else
            {
                dsAllGroups = mySecurityDB.getObjectGroups();
            }
        }
        public string TableName
        {
            get { return m_GroupTableName; }
            set { m_GroupTableName = value; }
        }

        public ArrayList Children(string Group)
        {
          /*
           * Creates a list of this Group's Children. 
           *  
           * Note that, since Groups can contain other groups, if the MemberType is cMTYPEGROUPE,
           * we recursively call Children for those groups and add them to the list if they are not
           * already there.
           */
          ArrayList aChildren = new ArrayList();  // list of Children we are going to return
          DataSet dsChildren;                     // rs with Group's immediate children
          int i;
          if (m_GroupTableName == tblGroups)  // This routine works for either SystemGroups or SystemObjectGroups
            dsChildren = mySecurityDB.getGroupChildren(Group);
          else
            dsChildren = mySecurityDB.getObjectGroupChildren(Group);
          for (i=0; i < dsChildren.Tables[0].Rows.Count ; i++) // For each of the immediate Children
          {
            if (dsChildren.Tables[0].Rows[i]["MemberType"].ToString() == cMTYPEGROUP) // is this Child a Group?
            {
              string thisMember = dsChildren.Tables[0].Rows[i]["Member"].ToString().ToLower();
              // Yes it is a Group. So, let's recursively call myself to get the Children of THAT group
              SecurityGroups mySG = new SecurityGroups(m_GroupTableName);
              ArrayList newChildren = mySG.Children(thisMember);
              int j;
              for (j = 0; j < newChildren.Count; j++) // and now add these to the list
              {
                if (aChildren.IndexOf(newChildren[j]) == -1)  // is it not already in the list? (IndexOf returns -1 if not found)
                    aChildren.Add(newChildren[j]);          // Then add it
              } 
            }
            else
            {
              // Nope, just a normal Child. Add it to the list
              string thisMember = dsChildren.Tables[0].Rows[i]["Member"].ToString().ToLower();
              if (aChildren.IndexOf(thisMember) == -1) // Is it not already on the list?
                aChildren.Add(thisMember);          // Then add it
            }
          }
          aChildren.Sort();   // Now sort the list for convenience
          return aChildren;   // And return it
        }
        public ArrayList Parents(string Member)
        {
            /*
             * Creates a list of this Member's Parents. 
             *  
             * Note that, since Groups can contain other groups, if the MemberType is cMTYPEGROUPE,
             * we recursively call Parents for those groups and add them to the list if they are not
             * already there.
             */
            ArrayList aParents = new ArrayList();  // list of Parents we are going to return
            DataSet dsParents; // rs with Member's immediate Parents
            int i;
            if (m_GroupTableName == tblGroups)  // This routine works for either SystemGroups or SystemObjectGroups
            {
                dsParents = mySecurityDB.getMemberParents(Member);
            }
            else
            {
                dsParents = mySecurityDB.getObjectMemberParents(Member);
            }
            for (i = 0; i < dsParents.Tables[0].Rows.Count; i++) // For each of the immediate Parents
            {
                // Everything is a group, so we first add it to the list (if it is not already there)
              string thisParent = dsParents.Tables[0].Rows[i]["sGroup"].ToString().ToLower();
              if (aParents.IndexOf(thisParent) == -1) // Is it not already on the list?
                {
                  aParents.Add(thisParent);          // Then add it
                }
                // Now we look to see if it this group is a member of some other group

                ArrayList newParents;
                newParents = Parents(thisParent);
                int j;
                for (j = 0; j < newParents.Count; j++) // and now add these to the list
                {
                    if (aParents.IndexOf(newParents[j]) == -1)  // is it not already in the list? (IndexOf returns -1 if not found)
                    {
                        aParents.Add(newParents[j]);          // Then add it
                    }
                }
            }
            aParents.Sort();   // Now sort the list for convenience
            return aParents;   // And return it
        }
    }
}
