using System;
using Zenject;

namespace Source.Scripts.Services.ScenesManagement
{
  public class GameSceneService : IGameSceneService
  {
    private const string CoreGameScene = "GamePlayScene";
    private const string LoadingScene = "LoadScene";

    private readonly SceneLoader _sceneLoader;

    [Inject]
    public GameSceneService(SceneLoader sceneLoader) => 
      _sceneLoader = sceneLoader;

    public void LoadMainGameScene(Action onLoad, float minWaitingTime = 0) => 
      _sceneLoader.Load(CoreGameScene, minWaitingTime, onLoad);

    public void ReloadGame(Action onLoad = null, float minWaitingTime = 0) => 
      _sceneLoader.Load(LoadingScene, minWaitingTime, onLoad);

  }
}