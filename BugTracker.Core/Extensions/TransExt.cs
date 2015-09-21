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

            string assemblyFilename = Path.GetFileNameWithoutExtension(assembly.CodeBase);
            string methodName = CleanString(method.ReflectedType.Name+ "_" + method.Name);

            return LocalizationManager.Instance.GetTranslation(LocalizationManager.Instance.ActiveUICulture, assemblyFilename, methodName, value).Translated;
        }

        public static string Tr(this string value, string comment)
        {
            Assembly assembly = Assembly.GetCallingAssembly();

            StackTrace stackTrace = new StackTrace();
            MethodBase method = stackTrace.GetFrame(1).GetMethod();

            string assemblyFilename = Path.GetFileNameWithoutExtension(assembly.CodeBase);
            string methodName = CleanString(method.ReflectedType.Name + "_" + method.Name);

            return LocalizationManager.Instance.GetTranslation(LocalizationManager.Instance.ActiveUICulture, assemblyFilename, methodName, value, comment).Translated;
        }

        private static string CleanString(string value)
        {
            Regex reg = new Regex("[\\W]");
            string result = reg.Replace(value, "_");

            while (result.Contains("__"))
            {
                result = result.Replace("__", "_");
            }

            return result;
        }
    }
}
