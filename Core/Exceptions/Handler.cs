using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AppCore.Exceptions
{
    public static class Handler
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
