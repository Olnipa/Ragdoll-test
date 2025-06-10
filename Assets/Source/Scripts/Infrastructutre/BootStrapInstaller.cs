using Source.Scripts.Services.Input;
using Source.Scripts.Services.ScenesManagement;
using Zenject;

namespace Source.Scripts.Infrastructutre
{
  public class BootStrapInstaller : MonoInstaller
  {
    private readonly CoroutineInvoker _coroutineInvoker;

    public override void InstallBindings()
    {
      BindInputService();
      BindGameSceneService();
      BindSceneLoader();
      BindCoroutineInvoker();
      // BindUpdaters();
    }

    private void BindUpdaters() =>
      Container
        .Bind<IFixedUpdater>()
        .FromInstance(_coroutineInvoker)
        .AsSingle();

    private void BindInputService() =>
      Container
        .BindInterfacesTo<InputService>()
        .AsSingle();

    private void BindGameSceneService() =>
      Container
        .BindInterfacesTo<GameSceneService>()
        .AsSingle();

    private void BindSceneLoader() =>
      Container
        .Bind<SceneLoader>()
        .To<SceneLoader>()
        .AsSingle();

    private void BindCoroutineInvoker() =>
      Container
        .BindInterfacesAndSelfTo<CoroutineInvoker>()
        .FromInstance(CoroutineInvoker.Instance)
        .AsSingle();
  }
}