using UnityEngine;

namespace Source.Scripts.Environment.HookDrag
{
  [RequireComponent(typeof(Rigidbody))]
  public class Hook : MonoBehaviour
  {
    [field: SerializeField] public Rigidbody RigidBody;

    private void Reset()
    {
      RigidBody = GetComponent<Rigidbody>();
    }
  }
}