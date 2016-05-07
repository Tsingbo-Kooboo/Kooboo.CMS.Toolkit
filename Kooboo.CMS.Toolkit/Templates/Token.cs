using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Toolkit.Templates
{
    public class Token
    {
        public const string DefaultPrefix = "{";
        public const string DefaultSuffix = "}";

        public Token(string prefix, string suffix)
        {
            Prefix = prefix;
            Suffix = suffix;
        }

        public string Prefix
        {
            get;
            private set;
        }

        public string Suffix
        {
            get;
            private set;
        }

        public string From(string key)
        {
            return Prefix + key + Suffix;
        }

        public string To(string key)
        {
            return Prefix + "/" + key + Suffix;
        }
    }
}