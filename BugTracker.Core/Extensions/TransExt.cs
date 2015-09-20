using BugTracker.Core.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;

namespace BugTracker.Core.Extensions
{
    public static class TransExt
    {
        public static string Tr(this string value)
        {
            Assembly assembly = Assembly.GetCallingAssembly();

            StackTrace stackTrace = new StackTrace();
            MethodBase method = stackTrace.GetFrame(1).GetMethod();

            return LocalizationManager.Instance.GetTranslation(assembly, method, value);
        }

        public static string Tr(this string value, string comment)
        {
            Assembly assembly = Assembly.GetCallingAssembly();

            StackTrace stackTrace = new StackTrace();
            MethodBase method = stackTrace.GetFrame(1).GetMethod();

            return LocalizationManager.Instance.GetTranslation(assembly, method, value, comment);
        }
    }
}
