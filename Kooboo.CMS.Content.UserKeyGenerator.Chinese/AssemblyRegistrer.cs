using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.UserKeyGenerator.Chinese
{
    public class AssemblyRegistrer : IDependencyRegistrar
    {
        public int Order
        {
            get
            {
                return 100;
            }
        }

        public void Register(IContainerManager containerManager, ITypeFinder typeFinder)
        {
            Kooboo.CMS.Content.Models.UserKeyGenerator.DefaultGenerator = new Kooboo.CMS.Content.UserKeyGenerator.Chinese.UserKeyGenerator();
        }
    }
}
