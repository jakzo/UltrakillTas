using System;
using System.Collections.Generic;

namespace UnityTas.Tests {
public abstract class Test {
  public static List<Test> AllTests = new List<Test>();

  public Test() { AllTests.Add(this); }

  abstract public void Run();

  public static bool RunTests() {
    var numPassed = 0;
    foreach (var testClass in AllTests) {
      try {
        testClass.Run();
        numPassed++;
      } catch (Exception err) {
        Console.Error.WriteLine($"Test failed: {testClass.ToString()}");
        Console.Error.WriteLine(err);
      }
    }
    Console.WriteLine($"{numPassed} / {AllTests.Count} tests passed");
    return numPassed == AllTests.Count;
  }
}

public static class Assert {
  public static new void Equals(object actual, object expected) {
    if (!actual.Equals(expected)) {
      throw new Exception(
          $"Expected: {actual.ToString()}\nBut got: {expected.ToString()}");
    }
  }
}
}
