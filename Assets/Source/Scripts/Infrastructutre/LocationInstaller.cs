using Source.Scripts.Environment;
using Source.Scripts.Environment.Camera;
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
    [SerializeField] private LineDrawer _lineDrawer;
    [SerializeField] private HUD _hud;
    [SerializeField] private CameraShakeConfig _cameraShakeConfig;

    public override void InstallBindings()
    {
      BindCamera();
      BindJointDetector();
      
      BindHook();
      BindLineDrawer();

      BindHUD();
      BindEndLevelLogic();
    }

    private void BindEndLevelLogic()
    {
      Container
        .BindInterfacesAndSelfTo<EndLevelListener>()
        .AsSingle()
        .NonLazy();
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

    private void BindCamera()
    {
      Container
        .Bind<CameraShakeConfig>()
        .FromInstance(_cameraShakeConfig)
        .AsSingle();

      Container.Bind<Camera>()
        .FromInstance(Camera.main)
        .AsSingle();
      
      Container
        .Bind<CameraShaker>()
        .To<CameraShaker>()
        .AsSingle();
    }

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