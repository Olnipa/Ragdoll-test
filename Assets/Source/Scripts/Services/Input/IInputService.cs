using System;
using UnityEngine;

namespace Source.Scripts.Services.Input
{
  public interface IInputService
  {
    event Action<Vector2> Dragged;
    event Action ClickReleased;
    event Action<Vector2> Clicked;
  }
}