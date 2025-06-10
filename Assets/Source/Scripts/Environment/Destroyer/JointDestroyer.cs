using Source.Scripts.RagdollLogic;
using Source.Scripts.Services.JointsDetector;
using UnityEngine;

namespace Source.Scripts.Environment.Destroyer
{
  public class JointDestroyer : MonoBehaviour
  {
    [SerializeField] private float _minImpactForce = 30f;
    
    private IJointsDetector _jointColliderDetector;

    private void OnCollisionEnter(Collision other)
    {
      IJointable jointable = other.gameObject.GetComponentInParent<IJointable>();

      if (jointable == null)
        return;

      if (other.relativeVelocity.magnitude > _minImpactForce) 
        jointable.Break();
    }
  }
}