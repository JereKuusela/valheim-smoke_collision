using BepInEx;
using HarmonyLib;
using UnityEngine;
namespace SmokeCollision;
[BepInPlugin("valheim.jere.smoke_collision", "SmokeCollision", "1.1.0.0")]
public class SmokeCollision : BaseUnityPlugin {
  ServerSync.ConfigSync ConfigSync = new("valheim.jere.smoke_collision")
  {
    DisplayName = "SmokeCollision",
    CurrentVersion = "1.1.0",
    MinimumRequiredVersion = "1.1.0"
  };
  public void Awake() {
    Configuration.Init(ConfigSync, Config);
    Harmony harmony = new("valheim.jere.smoke_collision");
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
  }
}
