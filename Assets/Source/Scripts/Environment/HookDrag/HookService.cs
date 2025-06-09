using System;
using Source.Scripts.RagdollLogic;
using Source.Scripts.Services.Input;
using Source.Scripts.Services.JointsDetector;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Source.Scripts.Environment.HookDrag
{
  public class HookService : IDisposable
  {
    private readonly IInputService _input;
    private readonly IJointsDetector _jointsDetector;
    private readonly Hook _hook;
    private readonly Camera _camera;
    private readonly LineDrawer _lineDrawer;

    private bool _isHooked;
    private ConfigurableJoint _selectedConfigurableJoint;

    [Inject]
    public HookService(IInputService input, IJointsDetector jointsDetector, Hook hook, Camera camera, LineDrawer lineDrawer)
    {
      _jointsDetector = jointsDetector;
      _hook = hook;
      _input = input;
      _camera = camera;
      _lineDrawer = lineDrawer;

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
      Object.Destroy(_selectedConfigurableJoint);
      _lineDrawer.RemoveLine();
    }

    private void OnDrag(Vector2 point)
    {
      if (!_isHooked) 
        return;
      
      Vector3 pointWorldPosition = GetPointWorldPosition(point);
      
      _hook.RigidBody.position = new Vector3(pointWorldPosition.x, pointWorldPosition.y, 0);
      _lineDrawer.DrawLine(_selectedConfigurableJoint.transform, pointWorldPosition);
    }

    private Vector3 GetPointWorldPosition(Vector2 point)
    {
      Ray ray = _camera.ScreenPointToRay(point);
      Vector3 pointWorldPosition = ray.origin + ray.direction * Mathf.Abs(_camera.transform.position.z);
      return pointWorldPosition;
    }

    private void OnColliderDetected(Collider collider, Vector3 point)
    {
      if (!collider.TryGetComponent(out _selectedConfigurableJoint))
        _selectedConfigurableJoint = collider.gameObject.AddComponent<ConfigurableJoint>();

      if (_selectedConfigurableJoint.TryGetComponent(out Rigidbody rigidbody))
      {
        rigidbody.angularVelocity = Vector3.zero;
        rigidbody.linearVelocity = Vector3.zero;
      }
      
      SetJointParameters(point);
      _lineDrawer.DrawLine(_selectedConfigurableJoint.transform, point);

      _isHooked = true;
    }

    private void SetJointParameters(Vector3 point)
    {
      _selectedConfigurableJoint.autoConfigureConnectedAnchor = false;
      Vector3 localAnchor = _selectedConfigurableJoint.transform.InverseTransformPoint(point);
      _selectedConfigurableJoint.anchor = localAnchor;
      _selectedConfigurableJoint.connectedAnchor = Vector3.zero;
      _selectedConfigurableJoint.targetPosition = Vector3.zero;
      
      _hook.RigidBody.position = point;
      _selectedConfigurableJoint.connectedBody = _hook.RigidBody;

      SetLinearDrives();

      _selectedConfigurableJoint.xMotion = ConfigurableJointMotion.Free;
      _selectedConfigurableJoint.yMotion = ConfigurableJointMotion.Free;
      _selectedConfigurableJoint.zMotion = ConfigurableJointMotion.Locked;

      SetAngularDrives();

      _selectedConfigurableJoint.angularXMotion = ConfigurableJointMotion.Free;
      _selectedConfigurableJoint.angularYMotion = ConfigurableJointMotion.Locked;
      _selectedConfigurableJoint.angularZMotion = ConfigurableJointMotion.Free;

      if (_selectedConfigurableJoint.TryGetComponent(out Rigidbody rb))
        rb.angularDamping = 5f;
    }

    private void SetLinearDrives()
    {
      JointDrive drive = new JointDrive
      {
        positionSpring = 8000,
        positionDamper = 2000,
        maximumForce = Mathf.Infinity
      };

      _selectedConfigurableJoint.xDrive = drive;
      _selectedConfigurableJoint.yDrive = drive;
      _selectedConfigurableJoint.zDrive = new JointDrive();
    }

    private void SetAngularDrives()
    {
      JointDrive angularDrive = new JointDrive
      {
        positionSpring = 5000,
        positionDamper = 1000,
        maximumForce = Mathf.Infinity
      };

      _selectedConfigurableJoint.angularXDrive = angularDrive;
      _selectedConfigurableJoint.angularYZDrive = angularDrive;
    }
  }
}