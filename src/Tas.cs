using System;
using UnityEngine;

namespace UnityTas {
public class Tas {
  private const string NAME = "UnityTas";

  public static void Init() {
    if (GameObject.Find(NAME))
      return;
    var go = new GameObject(NAME);
    go.AddComponent<Pauser>();
    go.AddComponent<StateSaver>();
  }
}
}
