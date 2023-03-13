using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;

namespace UnityTas {
public class StateSaver : MonoBehaviour {
  private Assembly _gameAssembly;
  private const BindingFlags _fieldFlags =
      BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static |
      BindingFlags.Instance | BindingFlags.DeclaredOnly;

  public static Assembly GetAssemblyByName(string name) {
    return AppDomain.CurrentDomain.GetAssemblies().First(
        assembly => assembly.GetName().Name == name);
  }

  public void Awake() { _gameAssembly = GetAssemblyByName("Assembly-CSharp"); }

  public GameManifest
  GenerateGameManifest(Assembly assembly = null) => new GameManifest() {
    game = Platform.Instance.GetGameInfo(),
    state = (assembly ?? _gameAssembly)
                .GetTypes()
                .Where(type => type.GetFields(_fieldFlags).Any())
                .ToDictionary(type => type.FullName,
                              type => new GameManifest.State() {
                                staticFields = GetDefaultFields(type, true),
                                nonstaticFields = GetDefaultFields(type, false),
                              }),
  };

  private Dictionary<string, GameManifest.Behavior>
  GetDefaultFields(Type type, bool staticFields) =>
      type.GetFields(_fieldFlags)
          .Where(field => field.IsStatic == staticFields)
          .ToDictionary(field => field.Name,
                        field => GameManifest.Behavior.UNKNOWN);
}
}
