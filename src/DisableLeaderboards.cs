using System;
using System.Threading.Tasks;
using HarmonyLib;

namespace UnityTas {
public class DisableLeaderboards {
  [HarmonyPatch(typeof(Steamworks.Data.Leaderboard),
                nameof(Steamworks.Data.Leaderboard.SubmitScoreAsync))]
  class Leaderboard_SubmitScoreAsync_Patch {
    [HarmonyPrefix()]
    internal static bool
    Prefix(ref Task<Steamworks.Data.LeaderboardUpdate?> __result) {
      __result = Task.Run<Steamworks.Data.LeaderboardUpdate?>(
          () => null as Steamworks.Data.LeaderboardUpdate?);
      return false;
    }
  }

  [HarmonyPatch(typeof(Steamworks.Data.Leaderboard),
                nameof(Steamworks.Data.Leaderboard.ReplaceScore))]
  class Leaderboard_ReplaceScore_Patch {
    [HarmonyPrefix()]
    internal static bool
    Prefix(ref Task<Steamworks.Data.LeaderboardUpdate?> __result) {
      __result = Task.Run<Steamworks.Data.LeaderboardUpdate?>(
          () => null as Steamworks.Data.LeaderboardUpdate?);
      return false;
    }
  }

  [HarmonyPatch(typeof(Steamworks.Data.Leaderboard),
                nameof(Steamworks.Data.Leaderboard.AttachUgc))]
  class Leaderboard_AttachUgc_Patch {
    [HarmonyPrefix()]
    internal static bool Prefix(ref Task<Steamworks.Result> __result) {
      __result = Task.Run<Steamworks.Result>(() => Steamworks.Result.OK);
      return false;
    }
  }

  [HarmonyPatch(typeof(GameProgressSaver),
                nameof(GameProgressSaver.SetBestCyber))]
  class GameProgressSaver_SetBestCyber_Patch {
    [HarmonyPrefix()]
    internal static bool Prefix() => false;
  }
}
}
