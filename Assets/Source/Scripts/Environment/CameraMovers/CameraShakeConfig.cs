using UnityEngine;

namespace Source.Scripts.Environment.Camera
{
  [CreateAssetMenu(fileName = "CameraShakerConfig", menuName = "Static Data/Camera Shaker Config")]
  public class CameraShakeConfig : ScriptableObject
  {
    public float Duration = 0.3f;
    public float Magnitude = 0.2f;
  }
}