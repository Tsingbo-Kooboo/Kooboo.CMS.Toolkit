using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Mvc;

namespace Kooboo.CMS.Toolkit.Plugins
{
    public class ExamplePlugin : PluginBase
    {
        public override ActionResult Execute()
        {
            if (IsPost) // Post back
            {
                // Business logic
            }

            return null;
        }
    }
}