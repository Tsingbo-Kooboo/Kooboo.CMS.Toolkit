using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Toolkit.Controls
{
    public class MediaImage : MediaFile
    {
        protected override string Validation
        {
            get
            {
                return "/.jpg$|.png$|.gif$|.bmp$|.jpeg$/i";
            }
        }

        protected override string ValidationErrorMessage
        {
            get
            {
                return "Please select a valid image";
            }
        }
    }
}