using System;
using System.Linq;
using System.Collections.Generic;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.Reflection;

namespace UnityTas {
public class GameManifest {
  public Game game;
  public Dictionary<string, State> state;

  public static GameManifest FromYaml(string yaml) {
    var deserializer =
        new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .WithTypeConverter(new YamlStringEnumConverter())
            .Build();
    return deserializer.Deserialize<GameManifest>(yaml);
  }

  public string ToYaml() {
    var serializer =
        new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .WithTypeConverter(new YamlStringEnumConverter())
            .Build();
    return serializer.Serialize(this);
  }

  public class YamlStringEnumConverter : IYamlTypeConverter {
    public bool Accepts(Type type) => type.IsEnum;

    public object ReadYaml(IParser parser, Type type) {
      var parsedEnum = parser.Consume<Scalar>();
      var serializableValues =
          type.GetMembers()
              .Select(m => new KeyValuePair<string, MemberInfo>(
                          m.GetCustomAttributes<YamlMemberAttribute>(true)
                              .Select(ema => ema.Alias)
                              .FirstOrDefault(),
                          m))
              .Where(pa => !String.IsNullOrEmpty(pa.Key))
              .ToDictionary(pa => pa.Key, pa => pa.Value);
      if (!serializableValues.ContainsKey(parsedEnum.Value)) {
        if (parsedEnum.Value == "null") {
          return null;
        }
        var value = parsedEnum.Value.Substring(0, 1).ToUpper() +
                    parsedEnum.Value.Substring(1);
        return Enum.Parse(type, value);
      }

      return Enum.Parse(type, serializableValues[parsedEnum.Value].Name);
    }

    public void WriteYaml(IEmitter emitter, object value, Type type) {
      var enumMember = type.GetMember(value.ToString()).FirstOrDefault();
      var yamlValue = enumMember?.GetCustomAttributes<YamlMemberAttribute>(true)
                          .Select(ema => ema.Alias)
                          .FirstOrDefault() ??
                      value.ToString();
      emitter.Emit(new Scalar(yamlValue));
    }
  }

  public class Game {
    public string name;
    public string developer;
  }

  public class State {
    [YamlMember(DefaultValuesHandling =
                    DefaultValuesHandling.OmitEmptyCollections,
                Alias = "static")]
    public Dictionary<string, Behavior> staticFields;
    [YamlMember(DefaultValuesHandling =
                    DefaultValuesHandling.OmitEmptyCollections,
                Alias = "nonstatic")]
    public Dictionary<string, Behavior> nonstaticFields;
  }

  public enum Behavior {
    [YamlMember(Alias = "unknown")] UNKNOWN,
    [YamlMember(Alias = "save")] SAVE,
    [YamlMember(Alias = "discard")] DISCARD,
  }
}
}
