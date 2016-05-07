using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Toolkit
{
    public class FolderUtility
    {
        public static TextFolder GetFolder(string folderFullName)
        {
            return GetFolder(Repository.Current, folderFullName);
        }

        public static TextFolder GetFolder(Repository repository, string folderFullName)
        {
            return new TextFolder(repository, folderFullName);
        }
    }
}