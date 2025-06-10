using System.Collections.Generic;
using Source.Scripts.RagdollLogic;
using UnityEngine;
using Zenject;

namespace Source.Scripts.Infrastructutre
{
  public class RagdollInstaller : MonoInstaller
  {
    [SerializeField] private List<Ragdoll> _ragdolls;
    [SerializeField] private RagdollDragConfig _ragdollDragConfig;

    public override void InstallBindings()
    {
      BindRagdoll();
      BindRagdollStabilizer();
      BindRagdollDragHelperConfig();
      BindRagdollDragHelper();
    }

    private void BindRagdollDragHelperConfig() =>
      Container
        .Bind<RagdollDragConfig>()
        .FromInstance(_ragdollDragConfig)
        .AsSingle();

    private void BindRagdollDragHelper() =>
      Container
        .BindInterfacesAndSelfTo<RagdollDragHelper>()
        .AsSingle();

    private void BindRagdollStabilizer() =>
      Container
        .BindInterfacesAndSelfTo<RagdollStabilizer>()
        .AsSingle();

    private void BindRagdoll() =>
      Container
        .BindInterfacesAndSelfTo<List<Ragdoll>>()
        .FromInstance(_ragdolls)
        .AsSingle();
  }
}