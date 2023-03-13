using System;
using System.Reflection;

namespace UnityTas {
public interface IPlatform {
  GameManifest.Game GetGameInfo();

  Assembly[] GetModRelatedAssemblies();
}
}
