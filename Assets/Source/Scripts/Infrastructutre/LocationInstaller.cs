using Source.Scripts.Services.JointsDetector;
using UnityEngine;
using Zenject;

namespace Source.Scripts.Infrastructutre
{
  public class LocationInstaller : MonoInstaller
  {
    [SerializeField] private JointDetectorConfig _jointConfig;

    public override void InstallBindings()
    {
      BindJointDetector();
      BindCamera();
    }

    private ConcreteIdArgConditionCopyNonLazyBinder BindCamera()
    {
      return Container.Bind<Camera>()
        .FromInstance(Camera.main)
        .AsSingle();
    }

    private void BindJointDetector()
    {
      Container
        .BindInterfacesTo<JointableColliderDetector>() // ← включает IDisposable
        .AsSingle()
        .NonLazy();

      Container.Bind<JointDetectorConfig>().FromInstance(_jointConfig).AsSingle();
    }
  }
}