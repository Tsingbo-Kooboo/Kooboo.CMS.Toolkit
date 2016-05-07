using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Toolkit.Controls
{
    public class MediaPdf : MediaFile
    {
        protected override string Validation
        {
            get
            {
                return "/.pdf$/i";
            }
        }

        protected override string ValidationErrorMessage
        {
            get
            {
                return "Please select a valid pdf";
            }
        }
    }
}