using BugTracker.Core.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BugTracker.Core.Classes
{
    public static class Exceptions
    {
        public static void Handle(Exception exc)
        {
            using (FormExceptionMessage dialog = new FormExceptionMessage(exc))
            {
                dialog.ShowDialog();
            }
        }
    }
}
