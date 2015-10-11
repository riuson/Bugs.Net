using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppCore.Classes
{
    internal class LookupBugWorkaround
    {
        /// <summary>
        /// Workaround for problem: [mscorlib recursive resource lookup bug]
        /// </summary>
        public LookupBugWorkaround()
        {
            /*
             * Expression: [mscorlib recursive resource lookup bug]
             * Description: Infinite recursion during lookup within mscorlib.
             *  This may be a bug in mscorlib, or potentially in certain extensibility points such as assembly resolve events or CultureInfi names.
             *  Resource name: Globalization.ci_en
             * 
             * See web:
             * https://social.msdn.microsoft.com/Forums/sqlserver/en-US/287e13d0-8387-42c9-ad0a-fea93c95c7fd/mscorlib-recursive-resource-lookup-bug?forum=wpf
             * https://connect.microsoft.com/VisualStudio/feedback/details/855833/mscorlib-recursive-resource-lookup-bug
             */
            try
            {
                throw new NotImplementedException(); // << THIS IS THE FIX
            }
            catch
            {
            }
        }
    }
}
