using MelonLoader;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

[assembly:AssemblyTitle(Utas.BuildInfo.Name)]
[assembly:AssemblyDescription("")]
[assembly:AssemblyConfiguration("")]
[assembly:AssemblyCompany(Utas.BuildInfo.Company)]
[assembly:AssemblyProduct(Utas.BuildInfo.Name)]
[assembly:AssemblyCopyright("Created by " + Utas.BuildInfo.Author)]
[assembly:AssemblyTrademark(Utas.BuildInfo.Company)]
[assembly:AssemblyCulture("")]
[assembly:ComVisible(false)]
//[assembly: Guid("")]
[assembly:AssemblyVersion(Utas.AppVersion.Value)]
[assembly:AssemblyFileVersion(Utas.AppVersion.Value)]
[assembly:NeutralResourcesLanguage("en")]
[assembly:MelonInfo(typeof(Utas.Mod), Utas.BuildInfo.Name,
                    Utas.AppVersion.Value, Utas.BuildInfo.Author,
                    Utas.BuildInfo.DownloadLink)]

[assembly:MelonGame(Utas.BuildInfo.Developer, Utas.BuildInfo.GameName)]
