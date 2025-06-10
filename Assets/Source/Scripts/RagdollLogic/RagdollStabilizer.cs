using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Source.Scripts.RagdollLogic
{
  public class RagdollStabilizer : IDisposable
  {
    private readonly RagdollDragConfig _dragConfig;
    private readonly Dictionary<Rigidbody, float> _cachedDrag = new();
    private readonly Dictionary<Rigidbody, float> _cachedAngularDrag = new();
    
    private List<Rigidbody> _ragdollBodies = new();
    
    private bool _isStabilized;

    [Inject]
    public RagdollStabilizer(RagdollDragConfig dragConfig)
    {
      _dragConfig = dragConfig;
    }

    public void Dispose() => 
      DisableStabilization();

    public void EnableStabilization(Ragdoll ragdoll)
    {
      if (_isStabilized)
        return;
      
      Initialize(ragdoll);
      
      foreach (Rigidbody body in _ragdollBodies)
      {
        body.linearDamping = _dragConfig.StabilizedDrag;
        body.angularDamping = _dragConfig.StabilizedAngularDrag;
      }
        
      _isStabilized = true;
    }

    public void DisableStabilization()
    {
      if (!_isStabilized) 
        return;
        
      foreach (Rigidbody body in _ragdollBodies.Where(body => body))
      {
        if (_cachedDrag.TryGetValue(body, out float linearDamping))
          body.linearDamping = linearDamping;
                
        if (_cachedAngularDrag.TryGetValue(body, out float angularDamping))
          body.angularDamping = angularDamping;
      }
        
      _isStabilized = false;
    }

    private void Initialize(Ragdoll ragdoll)
    {
      _ragdollBodies = ragdoll.Bodies;
      
      foreach (Rigidbody body in _ragdollBodies)
      {
        _cachedDrag[body] = body.linearDamping;
        _cachedAngularDrag[body] = body.angularDamping;
      }
    }
  }
}