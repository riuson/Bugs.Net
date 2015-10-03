using BugTracker.Core.Classes;
using BugTracker.Core.Localization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;

namespace BugTracker.Core.Extensions
{
    public static class TransExt
    {
        public static string Tr(this string value,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Assembly assembly = Assembly.GetCallingAssembly();

            string assemblyFilename = Path.GetFileNameWithoutExtension(assembly.CodeBase);

            return LocalizationManager.Instance.GetTranslation(
                LocalizationManager.Instance.ActiveUICulture,
                assemblyFilename,
                sourceFilePath,
                sourceLineNumber,
                memberName,
                value
                ).TranslatedString;
        }

        public static string Tr(this string value, string comment,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Assembly assembly = Assembly.GetCallingAssembly();

            string assemblyFilename = Path.GetFileNameWithoutExtension(assembly.CodeBase);


            return LocalizationManager.Instance.GetTranslation(
                LocalizationManager.Instance.ActiveUICulture,
                assemblyFilename,
                sourceFilePath,
                sourceLineNumber,
                memberName,
                value,
                comment
                ).TranslatedString;
        }
    }
}
