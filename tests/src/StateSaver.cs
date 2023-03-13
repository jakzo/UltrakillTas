using System;
using System.IO;
using UnityEngine;

namespace UnityTas.Tests {
public class _StateSaver {
#pragma warning disable CS0414, CS0169
  public class TestClass : TestClassParent {
    private static string _privStatStr = "private value";
    public static string PubStatStr;
    public string MyMethod() => null;
    public string MyProperty { get; set; }
    public string MyPubField;
    private string _myPrivField;
    public int MyIntField;
  }
  public class TestClassParent : MonoBehaviour {
    public string InheritedProp;
  }
  public class TestComponent2 : Component {}
#pragma warning restore CS0414, CS0169

  public class GetAssemblyByName : Test {
    public override void Run() {
      // List all assemblies
      // Console.WriteLine(
      //     string.Join("\n", AppDomain.CurrentDomain.GetAssemblies().Select(
      //                           assembly => $"->
      //                           {assembly.GetName().Name}")));

      Assert.Equals(StateSaver.GetAssemblyByName("UnityTasTests"),
                    typeof(GetAssemblyByName).Assembly);
    }
  }

  public class GenerateGameManifest : Test {
    public override void Run() {
      var yaml =
          new StateSaver()
              .GenerateGameManifest(typeof(GenerateGameManifest).Assembly)
              .ToYaml();
      Console.WriteLine($"GenerateGameManifest = {yaml}");
      Assert.Equals(yaml.Contains(@"
  UnityTas.Tests._StateSaver+TestClass:
    static:
      _privStatStr: unknown
      PubStatStr: unknown
    nonstatic:
      <MyProperty>k__BackingField: unknown
      MyPubField: unknown
      _myPrivField: unknown
      MyIntField: unknown
  UnityTas.Tests._StateSaver+TestClassParent:
    nonstatic:
      InheritedProp: unknown"),
                    true);
    }
  }

  public class GenerateRealManifest : Test {
    public override void Run() {
      LoadAssemblies(new[] {
        "Assembly-CSharp",
        "UnityEngine.CoreModule",
        "UnityEngine.UI",
        "Unity.InputSystem",
      });
      var stateSaver = new StateSaver();
      stateSaver.Awake();
      var yaml = stateSaver
                     .GenerateGameManifest(
                         StateSaver.GetAssemblyByName("UnityEngine.CoreModule"))
                     .ToYaml();
      System.IO.File.WriteAllText("manifest.yaml", yaml);
      Console.WriteLine("Generated real manifest");
    }

    private void LoadAssemblies(string[] names) {
      foreach (var name in names) {
        var asmBytes = System.IO.File.ReadAllBytes(
            $"./ULTRAKILL/ULTRAKILL_Data/Managed/{name}.dll");
        System.Reflection.Assembly.Load(asmBytes);
      }
    }
  }

  public class Serialize : Test {
    public static string MyStaticVar = "Hello";

    public override void Run() {
      var fs = File.Open("state.bin", FileMode.OpenOrCreate, FileAccess.Write,
                         FileShare.Read);
      StateSerializer.Run(fs);
      fs.Close();
    }
  }

  public class Deserialize : Test {
    public override void Run() {
      Serialize.MyStaticVar = "World";
      var fs = File.Open("state.bin", FileMode.Open, FileAccess.Read,
                         FileShare.Read);
      StateDeserializer.Run(fs);
      fs.Close();
      Assert.Equals(Serialize.MyStaticVar, "Hello");
    }
  }
}
}
