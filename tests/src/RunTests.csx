#r "../bin/Test/UnityTasTests.dll"

using UnityTas.Tests;

// new _StateSaver.GetAssemblyByName();
// new _StateSaver.GenerateGameManifest();
// new _StateSaver.GenerateRealManifest();
new _StateSaver.Serialize();
new _StateSaver.Deserialize();
var wasSuccessful = Test.RunTests();
Environment.Exit(wasSuccessful? 0: 1);
