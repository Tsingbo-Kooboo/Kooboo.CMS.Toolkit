using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Form.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Toolkit.Controls.FroalaEditor
{
    [Dependency(typeof(IHttpApplicationEvents), Order = 50, Key = "FroalaEditor")]
    public class ControlsInitializer : HttpApplicationEvents
    {
        public override void Application_Start(object sender, EventArgs e)
        {
            ControlHelper.RegisterControl(new FroalaEditorControl());
        }
    }
}
