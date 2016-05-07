using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;

namespace Kooboo.CMS.Toolkit
{
    public abstract class ContextBase
    {
        protected Hashtable _services = new Hashtable();

        public TService GetService<TService>()
            where TService : class
        {
            Type serviceType = typeof(TService);
            if (!_services.ContainsKey(serviceType))
            {
                TService service = Activator.CreateInstance<TService>();
                _services.Add(serviceType, service);
            }

            return (TService)_services[serviceType];
        }
    }
}