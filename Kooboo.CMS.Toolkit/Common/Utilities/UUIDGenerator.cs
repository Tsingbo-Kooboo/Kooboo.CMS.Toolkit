using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Toolkit
{
    public class UUIDGenerator
    {
        /// <summary>
        /// 16 digits
        /// </summary>
        /// <returns></returns>
        public static string Generate()
        {
            return Generate(16);
        }

        public static string Generate(int numDigits)
        {
            return UniqueIdGenerator.GetInstance().GetBase32UniqueId(numDigits);
        }
    }
}