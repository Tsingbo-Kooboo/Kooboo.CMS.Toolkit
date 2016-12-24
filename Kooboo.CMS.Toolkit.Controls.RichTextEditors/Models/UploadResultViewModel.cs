using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Toolkit.Controls.RichTextEditors.Models
{
    public class UploadResultViewModel
    {
        public string state { get; set; }
        public string url { get; set; }
        public string title { get; set; }
        public string original { get; set; }
        public string error { get; set; }
    }
}
