using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Models.Binder;
using Kooboo.CMS.Content.Persistence;
using Kooboo.CMS.Content.Services;

namespace Kooboo.CMS.Toolkit.Modules
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(Kooboo.CMS.Web.Areas.Contents.Controllers.TextContentController), Order = 50)]
    public class ModuleTextContentController : Kooboo.CMS.Web.Areas.Contents.Controllers.TextContentController
    {
        public ModuleTextContentController(TextContentManager textContentManager, TextFolderManager textFolderManager, WorkflowManager workflowManager, ITextContentBinder binder, ITextContentProvider textContentProvider)
            : base(textContentManager, textFolderManager, workflowManager, binder, textContentProvider)
         {
         }

        public string ModuleRepositoryName
        {
            get
            {
                return this.Request.QueryString["moduleRepositoryName"];
            }
        }

        private Repository _repository;
        public override Repository Repository
        {
            get
            {
                if(!String.IsNullOrEmpty(ModuleRepositoryName))
                {
                    if(_repository == null)
                    {
                        _repository = new Repository(ModuleRepositoryName);
                    }
                    return _repository;
                }
                else
                {
                    return base.Repository;
                }
            }
            set
            {
                if(!String.IsNullOrEmpty(ModuleRepositoryName))
                {
                    _repository = value;
                }
                else
                {
                    base.Repository = value;
                }
            }
        }
    }
}