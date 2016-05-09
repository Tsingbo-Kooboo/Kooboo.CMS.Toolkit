using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Membership.China.GraphDatas
{
    public class WeChatGraphData
    {
        public string openid { get; set; }

        public string nickname { get; set; }

        public string sex { get; set; }

        public string province { get; set; }

        public string city { get; set; }

        public string country { get; set; }

        public string headimgurl { get; set; }

        public string unionid { get; set; }

        public List<string> privilege { get; set; }
    }
}
