using BugTracker.Core.Classes;
using BugTracker.Core.Localization;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BugTracker.Core.Tests.Classes
{
    [TestFixture]
    internal class LocalizationManagerTest
    {
        [Test]
        public void CanTranslate()
        {
            CultureInfo cultureEn = new CultureInfo("en");
            CultureInfo cultureRu = new CultureInfo("ru");

            string assemblyFilename = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            string sourceFilePath = @"C:\Temp\File.cs";
            int sourceLineNumber = 31415926;
            string memberName = "CanTranslate";

            string str = LocalizationManager.Instance.GetTranslation(cultureEn, assemblyFilename, sourceFilePath, sourceLineNumber, memberName, "Test").TranslatedString;

            Assert.That(str, Is.EqualTo("Test"));

            LocalizationManager.Instance.GetTranslation(cultureRu, assemblyFilename, sourceFilePath, sourceLineNumber, memberName, "Test").TranslatedString = "Тест";

            str = LocalizationManager.Instance.GetTranslation(cultureRu, assemblyFilename, sourceFilePath, sourceLineNumber, memberName, "Test").TranslatedString;
            Assert.That(str, Is.EqualTo("Тест"));

            str = LocalizationManager.Instance.GetTranslation(cultureEn, assemblyFilename, sourceFilePath, sourceLineNumber, memberName, "Test").TranslatedString;
            Assert.That(str, Is.EqualTo("Test"));

            str = LocalizationManager.Instance.GetTranslation(cultureRu, assemblyFilename, sourceFilePath, sourceLineNumber, memberName, "Test").TranslatedString;
            Assert.That(str, Is.EqualTo("Тест"));

            //LocalizationManager.Instance.Save();
        }
    }
}
