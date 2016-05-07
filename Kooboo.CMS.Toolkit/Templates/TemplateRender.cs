using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using System.Text.RegularExpressions;

namespace Kooboo.CMS.Toolkit.Templates
{
    public class TemplateRender
    {
        public const string NamePattern = @"[a-zA-Z0-9_\.]+";

        public TemplateRender(string templateString)
            : this(Token.DefaultPrefix, Token.DefaultSuffix, templateString)
        { }

        public TemplateRender(string suffix, string prefix, string templateString)
            : this(new Token(suffix, prefix), templateString)
        { }

        public TemplateRender(Token token, string templateString)
        {
            Token = token;
            TokenPattern = Token.Prefix + "(" + NamePattern + ")" + Token.Suffix;
            TemplateString = templateString;
            Builder = new StringBuilder(TemplateString);
        }

        protected Token Token
        {
            get;
            private set;
        }

        protected string TokenPattern
        {
            get;
            private set;
        }

        protected string TemplateString
        {
            get;
            private set;
        }

        protected StringBuilder Builder
        {
            get;
            private set;
        }

        public TemplateRender Html(string key, string value)
        {
            Builder.Replace(Token.From(key), value);
            return this;
        }

        public TemplateRender Text(string key, string value)
        {
            Builder.Replace(Token.From(key), value.HtmlEncode());
            return this;
        }

        public TemplateRender Replace(params object[] data)
        {
            for (int i = 0; i < data.Length; i += 2)
            {
                string key = data[i].ToString();
                string value = data[i + 1].AsString();

                Html(key, value);
            }

            return this;
        }

        public TemplateRender Replace(object entity)
        {
            Type type = entity.GetType();

            foreach (Match item in Regex.Matches(Builder.ToString(), TokenPattern))
            {
                string key = item.Groups[1].Value;
                PropertyInfo property = type.GetProperty(key);
                if (property != null)
                {
                    string value = property.GetValue(entity, null).AsString();
                    Html(key, value);
                }
            }

            return this;
        }

        public TemplateRender Replace<T>(string key, IEnumerable<T> list)
        {
            string fromToken = Token.From(key);
            string toToken = Token.To(key);
            string pattern = fromToken + string.Format(@"([\s\S]*(?={0}))", toToken) + toToken;

            Match match = Regex.Match(Builder.ToString(), pattern);

            if (match.Success)
            {
                string itemTemplateString = match.Groups[1].Value;

                StringBuilder builder = new StringBuilder();
                foreach (var item in list)
                {
                    TemplateRender itemRender = new TemplateRender(itemTemplateString);
                    itemRender.Replace(item);
                    builder.Append(itemRender.ToString());
                }

                Builder.Replace(match.Value, builder.ToString());
            }

            return this;
        }

        public override string ToString()
        {
            return Builder.ToString();
        }
    }
}