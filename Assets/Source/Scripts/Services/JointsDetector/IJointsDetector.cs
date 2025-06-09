using System;
using Source.Scripts.RagdollLogic;
using UnityEngine;

namespace Source.Scripts.Services.JointsDetector
{
  public interface IJointsDetector
  {
    event Action<Collider, Vector3> ColliderDetected;
    event Action<Ragdoll> RagdollDetected;
  }
}