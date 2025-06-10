using System;
using System.Collections.Generic;
using System.Linq;
using Source.Scripts.Environment.Camera;
using Source.Scripts.RagdollLogic;
using UnityEngine;

namespace Source.Scripts.Environment
{
  public class EndLevelListener : IDisposable
  {
    private readonly List<Ragdoll> _ragdolls;
    private readonly CameraShaker _cameraShaker;

    public EndLevelListener(CameraShaker cameraShaker, List<Ragdoll> ragdolls)
    {
      _cameraShaker = cameraShaker;
      _ragdolls = ragdolls;
      
      Initialize();
    }

    private void Initialize()
    {
      foreach (Ragdoll ragdoll in _ragdolls) 
        ragdoll.Destroyed += OnRagdollDestroy;
    }

    private void OnRagdollDestroy()
    {
      _cameraShaker.StartShake();
      
      Ragdoll aliveRagdoll = _ragdolls.FirstOrDefault(ragdoll => !ragdoll.IsDestroyed);
      
      if (aliveRagdoll)
        return;
      
      Debug.Log("Destroyed All Ragdoll!");
    }

    public void Dispose()
    {
      foreach (Ragdoll ragdoll in _ragdolls) 
        ragdoll.Destroyed -= OnRagdollDestroy;
    }
  }
}