using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

using BibleVerses.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BibleVerses.Manager
{
    public class CodeProcessor : IDisposable
    {
        public void Dispose() { }
        public IEnumerable<CodeMasterModel> getCodes()
        {
            return DataSource.getCodes();
        }

    }
}
