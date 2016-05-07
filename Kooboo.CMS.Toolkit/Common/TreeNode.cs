using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace Kooboo.CMS.Toolkit
{
    [DataContract]
    public class TreeNode
    {
        [DataMember]
        public string Text
        {
            get;
            set;
        }

        [DataMember]
        public string Value
        {
            get;
            set;
        }

        private List<TreeNode> _children;
        [DataMember]
        public List<TreeNode> Children
        {
            get
            {
                if(_children == null)
                {
                    _children = new List<TreeNode>();
                }
                return _children;
            }
        }
    }
}