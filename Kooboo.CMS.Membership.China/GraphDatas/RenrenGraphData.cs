using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Membership.China.GraphDatas
{
    public class RenrenGraphData
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Image Avatar { get; set; }
    }

    public class Image
    {
        public string Url { get; set; }
        public ImageSize Size { get; set; }

        public enum ImageSize
        {
            MAIN,
            TINY,
            LARGE,
            HEAD
        }
    }
}
