using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.IO;

namespace UnityTas {
public static class FieldCache {
  private const BindingFlags ALL_FIELD_FLAGS =
      BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static |
      BindingFlags.Instance | BindingFlags.DeclaredOnly;

  // TODO: Ideally the serialization would work even with these assemblies
  private static bool IsBuiltinAssembly(Assembly assembly) {
    var name = assembly.GetName().Name;
    return name.StartsWith("System.") || name.StartsWith("Microsoft.") ||
           name.StartsWith("Mono.") || name == "netstandard" ||
           name == "mscorlib" || name == "csi" || name == "System";
  }

  public static Dictionary<string, Entry> Cache =
      new Dictionary<string, Entry>();

  public static void Populate() {
    var modRelatedAssemblies = Platform.Instance.GetModRelatedAssemblies();
    var gameAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(
        assembly => !modRelatedAssemblies.Contains(assembly) &&
                    !IsBuiltinAssembly(assembly));
    Console.WriteLine(
        $"ga = {string.Join(", ", gameAssemblies.Select(asm=>asm.GetName().Name))}");
    foreach (var assembly in gameAssemblies) {
      var name = assembly.GetName().Name;
      if (Cache.ContainsKey(name))
        continue;
      try {
        var types = assembly.GetTypes();
        Cache[name] = new Entry() {
          assembly = assembly,
          types = types,
          fields = types.SelectMany(type => type.GetFields(ALL_FIELD_FLAGS))
                       .ToArray(),
        };
      } catch {
        Console.WriteLine($"Failed to load types for assembly: {name}");
      }
    }
  }

  public class Entry {
    public Assembly assembly;
    public Type[] types;
    public FieldInfo[] fields;
  }
}
}
