using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Source.Scripts.RagdollLogic
{
  public class Ragdoll : MonoBehaviour, IJointable
  {
    [SerializeField] private List<CharacterJoint> _joints;
    [SerializeField] private List<Rigidbody> _rigidbodies;
    [SerializeField] private List<MeshRenderer> _renderers;

    [SerializeField] private float _limitInFreezeState;
    [SerializeField] private float _jointBreakForce = 80000;

    private readonly List<RagdollJointLimits> _defaultJointsLimits = new();

    private void Start()
    {
      foreach (CharacterJoint joint in _joints)
      {
        CacheDefaultLimits(joint);
        joint.breakForce = _jointBreakForce;
      }

      SetFreezeState();
    }

    public void Break()
    {
      SetZeroBrakeForce();
      SetGrayColor();
    }

    public void SetFreezeState()
    {
      foreach (var joint in _joints) 
        joint.swing1Limit = joint.swing2Limit = joint.highTwistLimit = joint.lowTwistLimit = new SoftJointLimit { limit = _limitInFreezeState };
    }

    public void SetDefaultState()
    {
      for (int i = 0; i < _joints.Count; i++)
      {
        CharacterJoint joint = _joints[i];
        joint.swing1Limit = _defaultJointsLimits[i].Swing1Limit;
        joint.swing2Limit = _defaultJointsLimits[i].Swing2Limit;
        joint.lowTwistLimit = _defaultJointsLimits[i].LowTwistLimit;
        joint.highTwistLimit = _defaultJointsLimits[i].HighTwistLimit;
      }
    }

    public void ResetVelocity()
    {
      foreach (Rigidbody rb in _rigidbodies) 
        rb.linearVelocity = Vector3.zero;
    }

    private void SetZeroBrakeForce()
    {
      foreach (CharacterJoint joint in _joints.Where(joint => joint)) 
        joint.breakForce = 0;
    }

    private void SetGrayColor()
    {
      foreach (MeshRenderer render in _renderers)
      {
        foreach (Material material in render.materials)
        {
          Color currentColor = material.color;
          float gray = currentColor.r * 0.299f + currentColor.g * 0.587f + currentColor.b * 0.114f;
          material.color = new Color(gray, gray, gray, currentColor.a);
        }
      }
    }

    private void CacheDefaultLimits(CharacterJoint joint)
    {
      _defaultJointsLimits.Add(new RagdollJointLimits(joint.swing1Limit, joint.swing2Limit, joint.highTwistLimit,
        joint.lowTwistLimit));
    }

    private void Reset()
    {
      _joints = GetComponentsInChildren<CharacterJoint>().ToList();
      _rigidbodies = GetComponentsInChildren<Rigidbody>().ToList();
      _renderers = _rigidbodies.Select(joint => joint.GetComponent<MeshRenderer>()).ToList();
    }
  }
}