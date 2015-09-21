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
            string assemblyFilename = Path.GetFileName(Assembly.GetExecutingAssembly().CodeBase);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            string str = LocalizationManager.Instance.GetTranslation(assemblyFilename, "CanTranslate", "Test");

            Assert.That(str, Is.EqualTo("Test"));

            Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru");
            LocalizationManager.Instance.SetTranslation(assemblyFilename, "CanTranslate", "Test", "Тест");

            Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru");
            str = LocalizationManager.Instance.GetTranslation(assemblyFilename, "CanTranslate", "Test");
            Assert.That(str, Is.EqualTo("Тест"));

            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
            str = LocalizationManager.Instance.GetTranslation(assemblyFilename, "CanTranslate", "Test");
            Assert.That(str, Is.EqualTo("Test"));

            Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru");
            str = LocalizationManager.Instance.GetTranslation(assemblyFilename, "CanTranslate", "Test");
            Assert.That(str, Is.EqualTo("Тест"));

            //LocalizationManager.Instance.Save();
        }
    }
}
