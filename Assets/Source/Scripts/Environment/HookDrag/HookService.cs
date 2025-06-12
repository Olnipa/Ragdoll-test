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
    private readonly UnityEngine.Camera _camera;
    private readonly LineDrawer _lineDrawer;
    private readonly RagdollStabilizer _ragdollStabilizer;
    private readonly RagdollDragHelper _ragdollDragHelper;
    private readonly RagdollDragConfig _config;

    private bool _isHooked;
    private ConfigurableJoint _selectedConfigurableJoint;
    private bool _isRagdollDetected;

    [Inject]
    public HookService(IInputService input, IJointsDetector jointsDetector, Hook hook, UnityEngine.Camera camera,
      LineDrawer lineDrawer, RagdollStabilizer ragdollStabilizer, RagdollDragHelper ragdollDragHelper, RagdollDragConfig config)
    {
      _jointsDetector = jointsDetector;
      _hook = hook;
      _input = input;
      _camera = camera;
      _lineDrawer = lineDrawer;
      _ragdollStabilizer = ragdollStabilizer;
      _ragdollDragHelper = ragdollDragHelper;
      _config = config;

      _jointsDetector.ColliderDetected += OnColliderDetected;
      _jointsDetector.RagdollDetected += OnRagdollDetected;
      _input.Dragged += OnDrag;
      _input.ClickReleased += OnDrop;
    }

    private void OnRagdollDetected(Ragdoll ragdoll)
    {
      _isRagdollDetected = true;
      ragdoll.SetDefaultState();
      _ragdollStabilizer.EnableStabilization(ragdoll);
      _ragdollDragHelper.Initialize(ragdoll);
    }

    public void Dispose()
    {
      _jointsDetector.ColliderDetected -= OnColliderDetected;
      _jointsDetector.RagdollDetected -= OnRagdollDetected;
      _input.Dragged -= OnDrag;
      _input.ClickReleased -= OnDrop;
    }

    private void OnDrop()
    {
      if (_selectedConfigurableJoint) 
        Object.Destroy(_selectedConfigurableJoint);

      if (_isRagdollDetected)
      {
        _ragdollStabilizer.DisableStabilization();
        _ragdollDragHelper.ClearTarget();
        _isRagdollDetected = false;
      }

      _lineDrawer.RemoveLine();
      _isHooked = false;
    }

    private void OnDrag(Vector2 point)
    {
      if (!_isHooked) 
        return;
      
      Vector3 pointWorldPosition = GetPointWorldPosition(point);
      
      _hook.RigidBody.position = new Vector3(pointWorldPosition.x, pointWorldPosition.y, 0);
      _lineDrawer.DrawLine(_selectedConfigurableJoint.transform, pointWorldPosition);
      
      if (_isRagdollDetected)
        _ragdollDragHelper.SetTarget(pointWorldPosition);
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
    }

    private void SetLinearDrives()
    {
      JointDrive drive = new JointDrive
      {
        positionSpring = _config.LinearSpring,
        positionDamper = _config.LinearDamper,
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
        positionSpring = _config.AngularSpring,
        positionDamper = _config.AngularDamper,
        maximumForce = Mathf.Infinity
      };

      _selectedConfigurableJoint.angularXDrive = angularDrive;
      _selectedConfigurableJoint.angularYZDrive = angularDrive;
    }
  }
}