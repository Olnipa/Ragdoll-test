using System;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Scripts.UI
{
  public class HUD : MonoBehaviour
  {
    [SerializeField] private Button _restartButton;
    
    public event Action RestartGameClick;

    private void Start() => 
      _restartButton.onClick.AddListener(OnRestartButtonCLick);

    private void OnRestartButtonCLick() => 
      RestartGameClick?.Invoke();

    private void OnDestroy() => 
      _restartButton.onClick.RemoveListener(OnRestartButtonCLick);

  }
}