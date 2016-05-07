using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Toolkit.Modules
{
    public interface IModuleInitializer
    {
        void Include(string moduleName, string siteName);

        void Exclude(string moduleName, string siteName);

        void UnInstall(string moduleName);
    }
}