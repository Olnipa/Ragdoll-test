using System;
using UnityEngine.InputSystem;

namespace Source.Scripts.Services.Input
{
  public interface IInputService
  {
    event Action<InputAction.CallbackContext> Dragged;
    event Action<InputAction.CallbackContext> ClickReleased;
    event Action<InputAction.CallbackContext> Clicked;
  }
}