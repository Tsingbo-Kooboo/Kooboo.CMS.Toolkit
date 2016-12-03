using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Toolkit.Controls.RichTextEditors.Models
{
    public class CrawlerViewModel
    {
        public string state { get; set; }

        public IEnumerable<CrawlerItemViewModel> list { get; set; }
    }

    public class CrawlerItemViewModel
    {
        public string state { get; set; }

        public string source { get; set; }

        public string url { get; set; }
    }
}
