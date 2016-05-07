using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Content.UserKeyGenerator.Chinese
{
    public class UserKeyGenerator : Kooboo.CMS.Content.Models.UserKeyGenerator
    {
        public override string Generate(Models.ContentBase content)
        {
            string userKey = content.UserKey;
            if (string.IsNullOrEmpty(userKey))
            {
                userKey = GetColumnValueForUserKey(content);
            }
            if (string.IsNullOrEmpty(userKey))
            {
                userKey = content.UUID;
            }
            else
            {
                string sperator = content.GetRepository().AsActual().UserKeyHyphens;
                userKey = PinyinConverter.GetPinyin(userKey, sperator);
                if (userKey.Length > 256)
                {
                    userKey = userKey.Substring(0, 256);
                }
                var tmpUserKey = EscapeUserKey(content, userKey);

                int tries = 0;
                while (IfUserKeyExists(content, tmpUserKey))
                {
                    tries++;
                    tmpUserKey = userKey + sperator + tries.ToString();
                }
                userKey = tmpUserKey;
            }

            return userKey;
        }
    }
}
