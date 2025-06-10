using System;
using System.Collections.Generic;
using Source.Scripts.Infrastructutre;
using UnityEngine;
using Zenject;

namespace Source.Scripts.RagdollLogic
{
  public class RagdollDragHelper : IDisposable
  {
    private readonly RagdollDragConfig _ragdollDragConfig;
    private readonly IFixedUpdater _fixedUpdater;
    
    private List<Rigidbody> _ragdollBodies;
    private Vector3 _targetPosition;
    private bool _hasTarget;

    [Inject]
    private RagdollDragHelper(RagdollDragConfig ragdollDragConfig, IFixedUpdater fixedUpdater)
    {
      _ragdollDragConfig = ragdollDragConfig;
      _fixedUpdater = fixedUpdater;
      
      _fixedUpdater.OnFixedUpdate += FixedUpdate;
    }

    public void Initialize(Ragdoll ragdoll)
    {
      _ragdollBodies = ragdoll.Bodies;
    }

    public void Dispose() => 
      _fixedUpdater.OnFixedUpdate -= FixedUpdate;

    public void SetTarget(Vector3 target)
    {
      _targetPosition = target;
      _targetPosition.z = 0;
      _hasTarget = true;
    }

    public void ClearTarget()
    {
      _hasTarget = false;
    }

    private void FixedUpdate()
    {
      if (_hasTarget) 
        ApplyMovementAssistance();
    }

    private void ApplyMovementAssistance()
    {
      // Debug.Log(_ragdollBodies.Count);
      foreach (Rigidbody body in _ragdollBodies)
      {
        float distanceToTarget = Vector3.Distance(body.transform.position, _targetPosition);

        if (distanceToTarget > _ragdollDragConfig.AssistanceRadius) 
          continue;
        
        Vector3 direction = (_targetPosition - body.transform.position).normalized;
        direction.z = 0;
                
        float forceMultiplier = 1f - distanceToTarget / _ragdollDragConfig.AssistanceRadius;
        forceMultiplier = Mathf.Clamp01(forceMultiplier);
                
        Vector3 force = direction * (_ragdollDragConfig.AssistanceForce * forceMultiplier);
        body.AddForce(force, ForceMode.Force);
      }
    }
  }
}