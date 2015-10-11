using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Управление общими сведениями о сборке осуществляется с помощью 
// набора атрибутов. Измените значения этих атрибутов, чтобы изменить сведения,
// связанные со сборкой.
[assembly: AssemblyTitle("BugTracker.DB")]
[assembly: AssemblyDescription("Database layer")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Параметр ComVisible со значением FALSE делает типы в сборке невидимыми 
// для COM-компонентов.  Если требуется обратиться к типу в этой сборке через 
// COM, задайте атрибуту ComVisible значение TRUE для этого типа.
[assembly: ComVisible(false)]

// Следующий GUID служит для идентификации библиотеки типов, если этот проект будет видимым для COM
[assembly: Guid("263d2527-29eb-4442-ada6-58d3445f3809")]

[assembly: AppCore.Plugins.AssemblyPluginType(typeof(BugTracker.DB.Classes.Plugin))]
[assembly: InternalsVisibleTo("BugTracker.DB.Tests, PublicKey=00240000048000009400000006020000002400005253413100040000010001001f687dda01dfaccd61b265b2b26c49e4346a6509ef38a9385905725f05ea9cde77b97c949d454b29560f81cd7c7dc28970999f97b9cdcd3b7594d9695143de87239e2ff06e64019ace11999e9d5377d3fbaf760ffe7a1077be89c3176b3daa8ca47c91255d184ccbcca6a5c14c9d683eef5a51891054c9b8f5dfea02b68cc8ae")]
