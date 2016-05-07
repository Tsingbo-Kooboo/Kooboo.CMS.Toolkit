using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.Toolkit.Results
{
    public class ImageResult : FilePathResult
    {
        public ImageResult(string imagePath)
            : base(imagePath, imagePath.GetImageContentType())
        {
        }
    }
}