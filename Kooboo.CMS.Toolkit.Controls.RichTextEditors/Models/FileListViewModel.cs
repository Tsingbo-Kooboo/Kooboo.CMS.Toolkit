using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Toolkit.Controls.RichTextEditors.Models
{
    public class FileListViewModel
    {
        public string state { get; set; }

        public IEnumerable<FileViewModel> list { get; set; }

        public int start { get; set; }

        public int total { get; set; }
    }

    public class FileViewModel
    {
        public string url { get; set; }

        public long mtime { get; set; }
    }

    public enum ResultState
    {
        Success,
        InvalidParam,
        AuthorizError,
        IOError,
        PathNotFound
    }
}
