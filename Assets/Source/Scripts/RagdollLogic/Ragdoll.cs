using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Source.Scripts.RagdollLogic
{
  public class Ragdoll : MonoBehaviour, IJointable
  {
    private static readonly Vector3 GrayLuminanceWeights = new(0.299f, 0.587f, 0.114f);
    
    [SerializeField] private List<RagdollPart> _joints;
    [SerializeField] private List<Rigidbody> _rigidbodies;
    [SerializeField] private List<MeshRenderer> _renderers;

    [SerializeField] private float _jointBreakForce = 80000;

    private readonly List<RagdollJointLimits> _defaultJointsLimits = new();
    
    public List<Rigidbody> Bodies => _rigidbodies.ToList();

    public bool IsDestroyed { get; private set; }

    public event Action Destroyed;

    private void Start()
    {
      foreach (RagdollPart ragdollPart in _joints)
      {
        CacheDefaultLimits(ragdollPart.Joint);
        ragdollPart.Joint.breakForce = _jointBreakForce;
        ragdollPart.JointBreak += Break;
      }

      SetFreezeState();
    }

    private void OnDestroy()
    {
      foreach (RagdollPart ragdollPart in _joints) 
        ragdollPart.JointBreak -= Break;
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

        CharacterJoint joint = _joints[i].Joint;
        joint.swing1Limit = _defaultJointsLimits[i].Swing1Limit;
        joint.swing2Limit = _defaultJointsLimits[i].Swing2Limit;
        joint.lowTwistLimit = _defaultJointsLimits[i].LowTwistLimit;
        joint.highTwistLimit = _defaultJointsLimits[i].HighTwistLimit;
      }
    }

    private void SetFreezeState()
    {
      foreach (RagdollPart ragdollPart in _joints)
      {
        CharacterJoint joint = ragdollPart.Joint;
        joint.swing1Limit = joint.swing2Limit = joint.highTwistLimit = joint.lowTwistLimit = new SoftJointLimit { limit = 0 };
      }
    }

    private void ResetConstrains()
    {
      foreach (Rigidbody rb in _rigidbodies) 
        rb.constraints = RigidbodyConstraints.None;
    }

    private void SetZeroBrakeForce()
    {
      foreach (RagdollPart joint in _joints.Where(joint => joint)) 
        joint.Joint.breakForce = 0;
    }

    private void SetGrayColor()
    {
      foreach (MeshRenderer render in _renderers)
      {
        foreach (Material material in render.materials)
        {
          Vector3 RGBcolor = new Vector3(material.color.r, material.color.g, material.color.b);
          float gray = Vector3.Dot(RGBcolor, GrayLuminanceWeights);
          material.color = new Color(gray, gray, gray, material.color.a);
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
      _joints = GetComponentsInChildren<RagdollPart>().ToList();
      _rigidbodies = GetComponentsInChildren<Rigidbody>().ToList();
      _renderers = _rigidbodies.Select(joint => joint.GetComponent<MeshRenderer>()).ToList();
    }
  }
}