﻿using System;
using System.Linq;
using Source.Scripts.RagdollLogic;
using Source.Scripts.Services.Input;
using UnityEngine;
using Zenject;
using Vector2 = UnityEngine.Vector2;

namespace Source.Scripts.Services.JointsDetector
{
  public class JointableColliderDetector : IJointsDetector, IDisposable
  {
    private readonly JointDetectorConfig _jointDetectorConfig;
    private readonly IInputService _input;
    private readonly Camera _camera;
    private readonly int _jointableColliderLayerMask;
    
    private readonly Collider[] _results = new Collider[7];

    public event Action<Ragdoll> RagdollDetected;
    public event Action<Collider, Vector3> ColliderDetected;

    [Inject]
    public JointableColliderDetector(IInputService input, Camera camera, JointDetectorConfig jointDetectorConfig)
    {
      _jointDetectorConfig = jointDetectorConfig;
      _input = input;
      _camera = camera;
      
      _jointableColliderLayerMask = 1 << _jointDetectorConfig.LayerMaskIndex;

      _input.Clicked += OnClick;
    }

    public void Dispose()
    {
      _input.Clicked -= OnClick;
    }

    private void OnClick(Vector2 position)
    {
      Ray ray = _camera.ScreenPointToRay(position);
      Vector3 point = ray.origin + ray.direction * Mathf.Abs(_camera.transform.position.z);
      
      ClearResults();

      if (Physics.OverlapSphereNonAlloc(point, _jointDetectorConfig.DetectorRadius, _results, _jointableColliderLayerMask) == 0)
        return;
     
      Collider collider = GetClosestCollider(point);

      if (!collider) 
        return;
      
      ColliderDetected?.Invoke(collider, point);
      
      Ragdoll ragdoll = collider.GetComponentInParent<Ragdoll>();
      
      if (ragdoll)
        RagdollDetected?.Invoke(ragdoll);
    }

    private Collider GetClosestCollider(Vector3 point) =>
      _results
        .Where(collider => collider)
        .OrderBy(collider => (point - collider.transform.position).sqrMagnitude)
        .FirstOrDefault();

    private void ClearResults()
    {
      for (int i = 0; i < _results.Length; i++) 
        _results[i] = null;
    }
  }
}