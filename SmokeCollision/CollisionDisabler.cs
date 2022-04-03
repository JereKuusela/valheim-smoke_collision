using UnityEngine;
namespace SmokeCollision;
public class CollisionDisabler : MonoBehaviour {
  private void OnCollisionEnter(Collision collision) {
    if (Configuration.IgnoredIds.Contains(Utils.GetPrefabName(collision.collider.gameObject).ToLower())) {
      Physics.IgnoreCollision(base.GetComponent<Collider>(), collision.collider, true);
    }
  }
}
