using System;
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

    [SerializeField] private float _jointBreakForce = 80000;

    private readonly List<RagdollJointLimits> _defaultJointsLimits = new();
    
    public List<Rigidbody> Bodies => _rigidbodies;

    public bool IsDestroyed { get; private set; }

    public event Action Destroyed;

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
      if (IsDestroyed)
        return;
      
      SetZeroBrakeForce();
      SetGrayColor();
      ResetConstrains();
      IsDestroyed = true;
      Destroyed?.Invoke();
    }

    public void SetDefaultState()
    {
      for (int i = 0; i < _joints.Count; i++)
      {
        if (!_joints[i])
          continue;
        
        _joints[i].swing1Limit = _defaultJointsLimits[i].Swing1Limit;
        _joints[i].swing2Limit = _defaultJointsLimits[i].Swing2Limit;
        _joints[i].lowTwistLimit = _defaultJointsLimits[i].LowTwistLimit;
        _joints[i].highTwistLimit = _defaultJointsLimits[i].HighTwistLimit;
      }
    }

    private void SetFreezeState()
    {
      foreach (CharacterJoint joint in _joints) 
        joint.swing1Limit = joint.swing2Limit = joint.highTwistLimit = joint.lowTwistLimit = new SoftJointLimit { limit = 0 };
    }

    private void ResetConstrains()
    {
      foreach (Rigidbody rb in _rigidbodies) 
        rb.constraints = RigidbodyConstraints.None;
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