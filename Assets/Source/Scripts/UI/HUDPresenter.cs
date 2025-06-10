using System;
using Source.Scripts.Services.ScenesManagement;
using Zenject;

namespace Source.Scripts.UI
{
  public class HUDPresenter : IDisposable
  {
    private readonly HUD _hud;
    private readonly IGameSceneService _sceneService;

    [Inject]
    public HUDPresenter(HUD hud, IGameSceneService sceneService)
    {
      _hud = hud;
      _sceneService = sceneService;
      
      Initialize();
    }

    private void Initialize()
    {
      _hud.RestartGameClick += OnRestartGameClick;
    }

    private void OnRestartGameClick()
    {
      _sceneService.ReloadGame();
    }

    public void Dispose()
    {
      _hud.RestartGameClick -= OnRestartGameClick;
    }
  }
}