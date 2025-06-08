using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Scripts.Services.Input
{
  public class InputService : IInputService, IDisposable
  {
    private readonly InputActions _inputActions;

    public event Action<Vector2> Dragged;
    public event Action ClickReleased;
    public event Action<Vector2> Clicked;

    public InputService()
    {
      _inputActions = new InputActions();
      _inputActions.Player.Enable();
      SubscribeUpdates();
    }

    private void SubscribeUpdates()
    {
      Debug.Log("InputService subscribed");
      _inputActions.Player.Press.performed += OnPlayerClick;
      _inputActions.Player.Release.performed += OnPlayerClickRelease;
      _inputActions.Player.Drag.performed += OnPlayerDrag;
    }

    private void OnPlayerDrag(InputAction.CallbackContext inputCallbackContext) => 
      Dragged?.Invoke(_inputActions.Player.Drag.ReadValue<Vector2>());

    private void OnPlayerClickRelease(InputAction.CallbackContext inputCallbackContext) => 
      ClickReleased?.Invoke();

    private void OnPlayerClick(InputAction.CallbackContext inputCallbackContext) => 
      Clicked?.Invoke(_inputActions.Player.Drag.ReadValue<Vector2>());

    public void Dispose()
    {
      _inputActions.Player.Press.performed -= OnPlayerClick;
      _inputActions.Player.Release.performed -= OnPlayerClickRelease;
      _inputActions.Player.Drag.performed -= OnPlayerDrag;
    }
  }
}