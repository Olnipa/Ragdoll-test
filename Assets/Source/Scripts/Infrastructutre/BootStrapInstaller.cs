using Source.Scripts.Services;
using Source.Scripts.Services.Input;
using Zenject;

namespace Source.Scripts.Infrastructutre
{
  public class BootStrapInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      BindInputService();
      BindGameSceneService();
      BindSceneLoader();
      BindCoroutineInvoker();
    }

    private void BindInputService()
    {
      Container
        .Bind<IInputService>()
        .To<InputService>()
        .AsSingle();
    }

    private void BindGameSceneService()
    {
      Container
        .Bind<IGameSceneService>()
        .To<GameSceneService>()
        .AsSingle();
    }

    private void BindSceneLoader() =>
      Container
        .Bind<SceneLoader>()
        .To<SceneLoader>()
        .AsSingle();

    private void BindCoroutineInvoker() =>
      Container
        .Bind<CoroutineInvoker>()
        .FromInstance(CoroutineInvoker.Instance)
        .AsSingle();
  }
}