using System;

namespace Source.Scripts.Services.ScenesManagement
{
  public interface IGameSceneService
  {
    void LoadMainGameScene(Action onLoad = null, float minWaitingTime = 0);
    void ReloadGame(Action onLoad = null, float minWaitingTime = 0);
  }
}