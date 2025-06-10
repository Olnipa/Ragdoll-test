using Source.Scripts.Environment.HookDrag;
using Source.Scripts.RagdollLogic;
using Source.Scripts.Services.JointsDetector;
using Source.Scripts.UI;
using UnityEngine;
using Zenject;

namespace Source.Scripts.Infrastructutre
{
  public class LocationInstaller : MonoInstaller
  {
    [SerializeField] private JointDetectorConfig _jointConfig;
    [SerializeField] private Hook _hook;
    [SerializeField] private Ragdoll _ragdoll;
    [SerializeField] private LineDrawer _lineDrawer;
    [SerializeField] private HUD _hud;

    public override void InstallBindings()
    {
      BindCamera();
      BindJointDetector();
      
      BindHook();
      BindRagdoll();
      BindLineDrawer();

      BindHUD();
    }

    private void BindHUD()
    {
      Container
        .Bind<HUD>()
        .FromInstance(_hud)
        .AsSingle();
      
      Container
        .BindInterfacesAndSelfTo<HUDPresenter>()
        .AsSingle()
        .NonLazy();
    }

    private void BindLineDrawer() =>
      Container
        .Bind<LineDrawer>()
        .FromInstance(_lineDrawer)
        .AsSingle();

    private void BindRagdoll() =>
      Container
        .BindInterfacesAndSelfTo<Ragdoll>()
        .FromInstance(_ragdoll)
        .AsSingle();

    private void BindHook()
    {
      Container
        .Bind<Hook>()
        .FromInstance(_hook)
        .AsSingle();

      Container
        .BindInterfacesAndSelfTo<HookService>()
        .AsSingle()
        .NonLazy();
    }

    private void BindCamera() =>
      Container.Bind<Camera>()
        .FromInstance(Camera.main)
        .AsSingle();

    private void BindJointDetector()
    {
      Container
        .Bind<JointDetectorConfig>()
        .FromInstance(_jointConfig)
        .AsSingle();

      Container
        .BindInterfacesTo<JointableColliderDetector>()
        .AsSingle();
    }
  }
}