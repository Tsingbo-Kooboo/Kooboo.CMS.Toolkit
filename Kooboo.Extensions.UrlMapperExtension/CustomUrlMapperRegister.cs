using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Extensions.UrlMapperExtension
{
    public class CustomUrlMapperRegister : Kooboo.CMS.Common.Runtime.IStartupTask
    {
        public void Execute()
        {
            Kooboo.CMS.Sites.Web.UrlMapperFactory.Default = new UrlMapperExtension();
        }

        public int Order
        {
            get { return 100; }
        }
    }
}
