using System;
using MelonLoader;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Linq;

[assembly:AssemblyTitle(UnityTas.BuildInfo.Name)]
[assembly:AssemblyDescription("")]
[assembly:AssemblyConfiguration("")]
[assembly:AssemblyCompany(UnityTas.BuildInfo.Company)]
[assembly:AssemblyProduct(UnityTas.BuildInfo.Name)]
[assembly:AssemblyCopyright("Created by " + UnityTas.BuildInfo.Author)]
[assembly:AssemblyTrademark(UnityTas.BuildInfo.Company)]
[assembly:AssemblyCulture("")]
[assembly:ComVisible(false)]
//[assembly: Guid("")]
[assembly:AssemblyVersion(UnityTas.AppVersion.Value)]
[assembly:AssemblyFileVersion(UnityTas.AppVersion.Value)]
[assembly:NeutralResourcesLanguage("en")]
[assembly:MelonInfo(typeof(UnityTas.Mod), UnityTas.BuildInfo.Name,
                    UnityTas.AppVersion.Value, UnityTas.BuildInfo.Author,
                    UnityTas.BuildInfo.DownloadLink)]

[assembly:MelonGame(UnityTas.BuildInfo.Developer, UnityTas.BuildInfo.GameName)]

namespace UnityTas {
public static class BuildInfo {
  public const string Name = "UltrakillTas";
  public const string Author = "jakzo";
  public const string Company = null;
  public const string Version = AppVersion.Value;
  public const string DownloadLink = "https://github.com/jakzo/UltrakillTas";

  public const string Developer = "Hakita";
  public const string GameName = "ULTRAKILL";
}

public class Mod : MelonMod {
  public override void OnSceneWasInitialized(int buildIndex, string sceneName) {
    Tas.Init();
  }
}

public static class Log {
  private static bool _isDebug =
#if __DEBUG__
      true
#else
      false
#endif
      ;

  public static void Debug(object data) {
    if (_isDebug)
      MelonLogger.Msg(data);
  }
  public static void Info(object data) => MelonLogger.Msg(data);
  public static void Warn(object data) => MelonLogger.Warning(data);
  public static void Error(object data) => MelonLogger.Error(data);
}

public class Platform : IPlatform {
  public static Platform Instance = new Platform();

  public GameManifest.Game GetGameInfo() => new GameManifest.Game() {
    name = MelonLoader.InternalUtils.UnityInformationHandler.GameName,
    developer = MelonLoader.InternalUtils.UnityInformationHandler.GameDeveloper,
  };

  public Assembly[] GetModRelatedAssemblies() =>
      // TODO: Does this include the assembly for MelonLoader itself?
      MelonAssembly.LoadedAssemblies.Select(asm => asm.Assembly).ToArray();
}
}
