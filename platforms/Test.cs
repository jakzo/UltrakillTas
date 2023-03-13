using System;
using System.Linq;
using System.Reflection;

namespace UnityTas {
public static class Log {
  public static void Debug(object data) {}
  public static void Info(object data) {}
  public static void Warn(object data) {}
  public static void Error(object data) {}
}

public class Platform : IPlatform {
  public static Platform Instance = new Platform();

  public GameManifest.Game GetGameInfo() => new GameManifest.Game() {
    name = "Test Game",
    developer = "Test Developer",
  };

  public Assembly[] GetModRelatedAssemblies() =>
      AppDomain.CurrentDomain.GetAssemblies()
          .Where(assembly => assembly.GetName().Name == "UnityTas")
          .ToArray();
}
}
