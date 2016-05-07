using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Toolkit
{
    public class RecursionVisitor
    {
        public static void Visit<T>(T model, Func<Action<T>, Action<T>> f)
        {
            Fix(f)(model);
        }

        private static Action<T> Fix<T>(Func<Action<T>, Action<T>> f)
        {
            return x => f(Fix(f))(x);
        }
    }
}