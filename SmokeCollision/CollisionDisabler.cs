using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;
namespace SmokeCollision;
public class CollisionDisabler : MonoBehaviour
{
  public static bool IsIgnored(Collider collider) => Configuration.IgnoredIds.Contains(Utils.GetPrefabName(collider.transform.root.gameObject).ToLower());
  private void OnCollisionEnter(Collision collision)
  {
    if (IsIgnored(collision.collider))
      Physics.IgnoreCollision(base.GetComponent<Collider>(), collision.collider, true);
  }

}

[HarmonyPatch(typeof(Fireplace), nameof(Fireplace.CheckUnderTerrain))]
public static class Patches
{

  static bool IsBlocked(Vector3 origin, Vector3 direction, out RaycastHit info, float maxDistance, int layerMask)
  {
    info = default;
    if (Configuration.RequiredSpace == 0f) return false;
    var hits = Physics.RaycastAll(origin, direction, Configuration.RequiredSpace, layerMask);
    return hits.Any(hit => !CollisionDisabler.IsIgnored(hit.collider));
  }
  static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
  {
    return new CodeMatcher(instructions)
      .MatchForward(false, new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(UnityEngine.Physics), nameof(UnityEngine.Physics.Raycast), new[] { typeof(Vector3), typeof(Vector3), typeof(RaycastHit).MakeByRefType(), typeof(float), typeof(int) })))
      .Set(OpCodes.Call, Transpilers.EmitDelegate(IsBlocked).operand)
      .InstructionEnumeration();
  }
}
