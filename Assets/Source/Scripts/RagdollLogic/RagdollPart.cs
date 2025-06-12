using System;
using UnityEngine;

namespace Source.Scripts.RagdollLogic
{
  public class RagdollPart : MonoBehaviour
  {
    [field: SerializeField] public CharacterJoint Joint { get; private set; }

    public event Action JointBreak;

    private void OnJointBreak(float breakForce) => 
      JointBreak?.Invoke();

    private void Reset() => 
      Joint = GetComponent<CharacterJoint>();
  }
}