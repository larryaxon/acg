using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleVerses.Common
{
    public sealed class SingletonDictionary
    {
        /*
         * To use, where key is a string:
            object val = SingletonDictionary.Instance.GetValue(key);
            or
            SingletonDictionary.Instance.SetValue(key, val);
        */
        private static volatile SingletonDictionary instance;
        private static object syncRoot = new Object();
        private Dictionary<string, object> _dict = null;

        private SingletonDictionary()
        {
            _dict = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
        }

        public static SingletonDictionary Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new SingletonDictionary();
                    }
                }

                return instance;
            }
        }
        public object GetValue(string key)
        {
            if (_dict.ContainsKey(key))
                return _dict[key];
            return null;
        }
        public void SetValue(string key, object val)
        {
            if (_dict.ContainsKey(key))
                _dict[key] = val;
            else
                _dict.Add(key, val);
        }
        
    }

}
