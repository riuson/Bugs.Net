using BugTracker.Core.Classes;
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
            string str = LocalizationManager.Instance.GetTranslation(cultureEn, assemblyFilename, "CanTranslate", "Test").Translated;

            Assert.That(str, Is.EqualTo("Test"));

            LocalizationManager.Instance.SetTranslation(cultureRu, assemblyFilename, "CanTranslate", "Test", "Тест");

            str = LocalizationManager.Instance.GetTranslation(cultureRu, assemblyFilename, "CanTranslate", "Test").Translated;
            Assert.That(str, Is.EqualTo("Тест"));

            str = LocalizationManager.Instance.GetTranslation(cultureEn, assemblyFilename, "CanTranslate", "Test").Translated;
            Assert.That(str, Is.EqualTo("Test"));

            str = LocalizationManager.Instance.GetTranslation(cultureRu, assemblyFilename, "CanTranslate", "Test").Translated;
            Assert.That(str, Is.EqualTo("Тест"));

            //LocalizationManager.Instance.Save();
        }
    }
}
