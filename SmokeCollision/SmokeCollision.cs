using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;
using BepInEx;
using HarmonyLib;
using UnityEngine;
namespace SmokeCollision;
[BepInPlugin(GUID, NAME, VERSION)]
public class SmokeCollision : BaseUnityPlugin {
  public const string LEGACY_GUID = "valheim.jere.smoke_collision";
  public const string GUID = "smoke_collision";
  public const string NAME = "Smoke Collision";
  public const string VERSION = "1.2";
  ServerSync.ConfigSync ConfigSync = new(GUID)
  {
    DisplayName = NAME,
    CurrentVersion = VERSION,
    MinimumRequiredVersion = VERSION
  };
  public void Awake() {
    var legacyConfig = Path.Combine(Path.GetDirectoryName(Config.ConfigFilePath), $"{LEGACY_GUID}.cfg");
    var config = Path.Combine(Path.GetDirectoryName(Config.ConfigFilePath), $"{GUID}.cfg");
    if (File.Exists(legacyConfig)) {
      if (File.Exists(config))
        File.Delete(legacyConfig);
      else
        File.Move(legacyConfig, config);
    }
    Configuration.Init(ConfigSync, Config);
    Harmony harmony = new(GUID);
    harmony.PatchAll();
  }
}

[HarmonyPatch(typeof(Smoke), nameof(Smoke.Awake))]
public class Smoke_Awake {
  static void Postfix(Smoke __instance) {
    __instance.gameObject.AddComponent<CollisionDisabler>();
    if (Configuration.SizeMultiplier != 1f)
      __instance.transform.localScale *= Configuration.SizeMultiplier;
    if (Configuration.ColliderMultiplier != 1f)
      __instance.GetComponent<SphereCollider>().radius *= Configuration.ColliderMultiplier;
    __instance.m_ttl = Configuration.TimeToDisappear;
    __instance.m_vel = new(0f, Configuration.VerticalVelocity, 0f);
    __instance.m_randomVel = Configuration.RandomHorizontalVelocity;
    __instance.m_vel += Quaternion.Euler(0f, (float)UnityEngine.Random.Range(0, 360), 0f) * Vector3.forward * __instance.m_randomVel;
    __instance.m_force = Configuration.Force;
    __instance.m_fadetime = Configuration.FadeTime;
  }
}

[HarmonyPatch(typeof(SmokeSpawner), nameof(SmokeSpawner.Spawn))]
public class SpawnSmoke {
  static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
    var matcher = new CodeMatcher(instructions).MatchForward(false, new CodeMatch(OpCodes.Ldc_I4_S, (sbyte)100))
      .SetAndAdvance(OpCodes.Call, Transpilers.EmitDelegate(() => Configuration.MaxAmount).operand);
    return matcher.InstructionEnumeration();
  }
}