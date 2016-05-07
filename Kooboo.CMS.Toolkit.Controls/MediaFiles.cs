using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Toolkit.Controls
{
    public class MediaFiles : MediaFile
    {
        protected override bool AllowMultipleFiles
        {
            get
            {
                return true;
            }
        }
    }
}