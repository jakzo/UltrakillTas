using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace UnityTas {
public class StateDeserializer {
  public static void Run(FileStream fs) {
    var deserializer = new StateDeserializer(fs);
    deserializer.Deserialize();
  }

  private FileStream _fs;

  private StateDeserializer(FileStream fs) { _fs = fs; }

  public void Deserialize() {
    FieldCache.Populate();
    var state =
        (Dictionary<FieldInfo, object>)new BinaryFormatter().Deserialize(_fs);
    foreach (var entry in state) {
      entry.Key.SetValue(null, entry.Value);
    }
  }
}
}
