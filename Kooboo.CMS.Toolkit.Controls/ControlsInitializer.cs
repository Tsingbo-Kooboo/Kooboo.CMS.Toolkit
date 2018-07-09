﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Form.Html;

namespace Kooboo.CMS.Toolkit.Controls
{
    [Dependency(typeof(Kooboo.CMS.Common.IHttpApplicationEvents), Key = "ControlsInitializer", Order = 50)]
    public class ControlsInitializer : Kooboo.CMS.Common.HttpApplicationEvents
    {
        public override void Application_Start(object sender, EventArgs e)
        {
            ControlHelper.RegisterControl(new DateTime());
            ControlHelper.RegisterControl(new UserSelector());
            ControlHelper.RegisterControl(new UsersSelector());
            ControlHelper.RegisterControl(new RoleSelector());
            ControlHelper.RegisterControl(new RolesSelector());
            ControlHelper.RegisterControl(new MemberSelector());
            ControlHelper.RegisterControl(new MembersSelector());
            ControlHelper.RegisterControl(new PageSelector());
            ControlHelper.RegisterControl(new PagesSelector());
            ControlHelper.RegisterControl(new ViewSelector());
            ControlHelper.RegisterControl(new ViewsSelector());
            ControlHelper.RegisterControl(new HtmlBlockSelector());
            ControlHelper.RegisterControl(new HtmlBlocksSelector());
            ControlHelper.RegisterControl(new MediaImage());
            ControlHelper.RegisterControl(new MediaImages());
            ControlHelper.RegisterControl(new MediaPdf());
            ControlHelper.RegisterControl(new MediaFile());
            //ControlHelper.RegisterControl(new MediaFiles());
            ControlHelper.RegisterControl(new CascadingDropdown());
            ControlHelper.RegisterControl(new TextFolderSelector());
            ControlHelper.RegisterControl(new TextFoldersSelector());
        }
    }
}