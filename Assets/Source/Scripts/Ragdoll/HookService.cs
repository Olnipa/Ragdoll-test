using System;
using Source.Scripts.Services.Input;
using Source.Scripts.Services.JointsDetector;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Source.Scripts.Ragdoll
{
  public class HookService : IDisposable
  {
    private readonly IInputService _input;
    private readonly IJointsDetector _jointsDetector;
    private readonly Hook _hook;
    private readonly Camera _camera;
    
    private bool _isHooked;
    private ConfigurableJoint _configurableJoint;

    [Inject]
    public HookService(IJointsDetector jointsDetector, Hook hook, IInputService input, Camera camera)
    {
      _jointsDetector = jointsDetector;
      _hook = hook;
      _input = input;
      _camera = camera;

      _jointsDetector.ColliderDetected += OnColliderDetected;
      _input.Dragged += OnDrag;
      _input.ClickReleased += OnDrop;
    }

    public void Dispose()
    {
      _jointsDetector.ColliderDetected -= OnColliderDetected;
      _input.Dragged -= OnDrag;
      _input.ClickReleased -= OnDrop;
    }

    private void OnDrop()
    {
      _isHooked = false;
      Object.Destroy(_configurableJoint);
    }

    private void OnDrag(Vector2 point)
    {
      if (_isHooked)
        _hook.transform.position = GetPointWorldPosition(point);
    }

    private Vector3 GetPointWorldPosition(Vector2 point)
    {
      Ray ray = _camera.ScreenPointToRay(point);
      Vector3 pointWorldPosition = ray.origin + ray.direction * Mathf.Abs(_camera.transform.position.z);
      return new Vector3(pointWorldPosition.x, pointWorldPosition.y, 0);
    }

    private void OnColliderDetected(Collider collider, Vector3 point)
    {
      if (!collider.TryGetComponent(out _configurableJoint))
        _configurableJoint = collider.gameObject.AddComponent<ConfigurableJoint>();
      
      _hook.RigidBody.position = point;
      _configurableJoint.connectedBody = _hook.RigidBody;

      JointDrive xdrive = _configurableJoint.xDrive;
      xdrive.positionSpring = 4000;
      xdrive.positionDamper = 700;
      _configurableJoint.xDrive = xdrive;

      JointDrive ydrive = _configurableJoint.yDrive;
      ydrive.positionSpring = 4000;
      ydrive.positionDamper = 700;
      _configurableJoint.yDrive = ydrive;

      JointDrive zdrive = _configurableJoint.zDrive;
      zdrive.positionSpring = 4000;
      zdrive.positionDamper = 700;
      _configurableJoint.zDrive = zdrive;

      _configurableJoint.rotationDriveMode = RotationDriveMode.Slerp;
      
      // JointDrive drive = _configurableJoint.slerpDrive;
      // drive.positionSpring = 4000;
      // drive.positionDamper = 700;
      // _configurableJoint.slerpDrive = drive;

      _configurableJoint.zMotion = ConfigurableJointMotion.Locked;
      _configurableJoint.angularYMotion = ConfigurableJointMotion.Locked;
      
      _isHooked = true;
    }
  }
}