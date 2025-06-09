using UnityEngine;

namespace Source.Scripts.RagdollLogic
{
  public class RagdollJointLimits
  {
    public SoftJointLimit Swing1Limit { get; private set; }
    public SoftJointLimit Swing2Limit { get; private set; }
    public SoftJointLimit HighTwistLimit { get; private set; }
    public SoftJointLimit LowTwistLimit { get; private set; }

    public RagdollJointLimits(SoftJointLimit swing1Limit, SoftJointLimit swing2Limit,
      SoftJointLimit highTwistLimit, SoftJointLimit lowTwistLimit)
    {
      Swing1Limit = swing1Limit;
      Swing2Limit = swing2Limit;
      HighTwistLimit = highTwistLimit;
      LowTwistLimit = lowTwistLimit;
    }
  }
}