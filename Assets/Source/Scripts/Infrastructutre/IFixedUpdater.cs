using System;

namespace Source.Scripts.Infrastructutre
{
  public interface IFixedUpdater
  {
    event Action OnFixedUpdate;
  }
}