using UnityEngine;

namespace Source.Scripts.Environment.Ball
{
  public class BouncingBall : MonoBehaviour
  {
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _maxForce = 18f;
    [SerializeField] private float _initialJumpForce = 5f;
    [SerializeField] private float _jumpForceIncrement = 1f;

    private float _currentJumpForce;
    private bool _isGrounded;

    void Start()
    {
      _currentJumpForce = _initialJumpForce;
    }

    private void OnCollisionEnter(Collision other)
    {
      if (!other.gameObject.TryGetComponent(out Ground _))
        return;

      _isGrounded = true;
    }

    private void OnCollisionExit(Collision other)
    {
      if (!other.gameObject.TryGetComponent(out Ground _))
        return;

      _isGrounded = false;
    }

    private void FixedUpdate()
    {
      if (!_isGrounded)
        return;

      AddForce();
      _isGrounded = false;
    }

    private void AddForce()
    {
      _rb.AddForce(Vector3.up * _currentJumpForce, ForceMode.Impulse);
      
      if (_currentJumpForce < _maxForce)
        _currentJumpForce += _jumpForceIncrement;
    }

    private void Reset()
    {
      _rb = GetComponent<Rigidbody>();
    }
  }
}