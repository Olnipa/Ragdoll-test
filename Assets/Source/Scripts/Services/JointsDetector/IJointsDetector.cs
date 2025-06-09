using System;
using UnityEngine;

namespace Source.Scripts.Services.JointsDetector
{
  public interface IJointsDetector
  {
    event Action<Collider> ColliderDetected;
  }
}