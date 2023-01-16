using System;
using MelonLoader;
using UnityEngine;

namespace Utas {
public static class BuildInfo {
  public const string Name = "UltrakillTas";
  public const string Author = "jakzo";
  public const string Company = null;
  public const string Version = AppVersion.Value;
  public const string DownloadLink =
      "https://ultrakill.thunderstore.io/package/jakzo/UltrakillTas/";

  public const string Developer = "Hakita";
  public const string GameName = "ULTRAKILL";
}

public class Mod : MelonMod {
  const string NAME = "Utas";

  public override void OnSceneWasInitialized(int buildIndex, string sceneName) {
    if (GameObject.Find(NAME))
      return;
    var go = new GameObject(NAME);
    go.AddComponent<Pauser>();
  }
}
}
