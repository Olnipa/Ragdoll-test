using UnityEngine;

namespace Source.Scripts.Services.JointsDetector
{
  [CreateAssetMenu(fileName = "JointDetectorConfig", menuName = "Static Data/JointDetector Config")]
  public class JointDetectorConfig : ScriptableObject
  {
    public int LayerMaskIndex = 3;
    public float DetectorRadius = 2f;
  }
}