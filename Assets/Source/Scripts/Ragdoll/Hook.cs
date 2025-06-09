using UnityEngine;

namespace Source.Scripts.Ragdoll
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