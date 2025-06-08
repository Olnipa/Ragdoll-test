using System;
using UnityEngine.InputSystem;

namespace Source.Scripts.Services.Input
{
  public class InputService : IDisposable, IInputService
  {
    private readonly InputActions _inputActions;

    public event Action<InputAction.CallbackContext> Dragged;
    public event Action<InputAction.CallbackContext> ClickReleased;
    public event Action<InputAction.CallbackContext> Clicked;

    public InputService()
    {
      _inputActions = new InputActions();
      SubscribeUpdates();
    }

    private void SubscribeUpdates()
    {
      _inputActions.Player.Press.performed += OnPlayerClick;
      _inputActions.Player.Release.performed += OnPlayerClickRelease;
      _inputActions.Player.Drag.performed += OnPlayerDrag;
    }

    private void OnPlayerDrag(InputAction.CallbackContext inputCallbackContext) => 
      Dragged?.Invoke(inputCallbackContext);

    private void OnPlayerClickRelease(InputAction.CallbackContext inputCallbackContext) => 
      ClickReleased?.Invoke(inputCallbackContext);

    private void OnPlayerClick(InputAction.CallbackContext inputCallbackContext) => 
      Clicked?.Invoke(inputCallbackContext);

    public void Dispose()
    {
      _inputActions.Player.Press.performed -= OnPlayerClick;
      _inputActions.Player.Release.performed -= OnPlayerClickRelease;
      _inputActions.Player.Drag.performed -= OnPlayerDrag;
    }
  }
}