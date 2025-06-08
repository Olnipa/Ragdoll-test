using Source.Scripts.Services.JointsDetector;
using Zenject;

namespace Source.Scripts.Infrastructutre
{
  public class LocationInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      BindJointDetector();
    }

    private void BindJointDetector() =>
      Container
        .BindInterfacesTo<JointsDetector>() // ← включает IDisposable
        .AsSingle()
        .NonLazy();
  }
}