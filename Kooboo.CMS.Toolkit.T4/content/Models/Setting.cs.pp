using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Toolkit;

namespace $RootNamespace$.Models
{
    public class Setting : TextContent
    {
        public Setting()
            : base(new TextContent())
        { }

        public Setting(TextContent textContent)
            : base(textContent)
        { }

        public class FieldNames
        {
            public const string SiteName = "SiteName";
            public const string Key = "Key";
            public const string Value = "Value";
        }

        /// <summary>
        /// Site full name
        /// </summary>
        public string SiteName
        {
            get
            {
                return this.GetString(FieldNames.SiteName);
            }
            set
            {
                this[FieldNames.SiteName] = value;
            }
        }

        public string Key
        {
            get
            {
                return this.GetString(FieldNames.Key);
            }
            set
            {
                this[FieldNames.Key] = value;
            }
        }

        public string Value
        {
            get
            {
                return this.GetString(FieldNames.Value);
            }
            set
            {
                this[FieldNames.Value] = value;
            }
        }
    }
}