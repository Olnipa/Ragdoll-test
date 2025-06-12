using UnityEngine;

namespace Source.Scripts.RagdollLogic
{
  [CreateAssetMenu(fileName = "RagdollDragConfig", menuName = "Static Data/Ragdoll Drag Config")]
  public class RagdollDragConfig : ScriptableObject
  {
    [Header("Drag Stabilizer")]
    [Min(0)] public float StabilizedDrag = 5f;
    [Min(0)] public float StabilizedAngularDrag = 10f;

    [Header("Drag Helper")]
    [Min(0)] public float AssistanceForce = 120f;
    [Min(0)] public float AssistanceRadius = 30f;

    [Header("Drag Joint")]
    [Min(0)] public float LinearSpring = 8000;
    [Min(0)] public float LinearDamper = 2000;
    [Min(0)] public float AngularSpring = 5000;
    [Min(0)] public float AngularDamper = 1000;
  }
}