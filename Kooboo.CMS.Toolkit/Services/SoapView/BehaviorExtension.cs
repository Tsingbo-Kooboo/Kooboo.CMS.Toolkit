using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ServiceModel.Configuration;

namespace Kooboo.CMS.Toolkit.Services.SoapView
{
    public class BehaviorExtension : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get { return typeof(EndpointBehavior); }
        }

        protected override object CreateBehavior()
        {
            return new EndpointBehavior();
        }
    }
}