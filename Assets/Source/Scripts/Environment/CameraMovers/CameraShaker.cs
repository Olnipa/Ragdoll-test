using System.Collections;
using Source.Scripts.Infrastructutre;
using UnityEngine;
using Zenject;

namespace Source.Scripts.Environment.Camera
{
  public class CameraShaker
  {
    private readonly Vector3 _originalPos;
    private readonly CoroutineInvoker _coroutineInvoker;
    private readonly UnityEngine.Camera _camera;
    private readonly CameraShakeConfig _cameraShakeConfig;
    
    private Coroutine _shakeCoroutine;

    [Inject]
    public CameraShaker(CoroutineInvoker coroutineInvoker, UnityEngine.Camera camera, CameraShakeConfig cameraShakeConfig)
    {
      _camera = camera;
      _coroutineInvoker = coroutineInvoker;
      _originalPos = camera.transform.position;
      _cameraShakeConfig = cameraShakeConfig;
    }
    
    public void StartShake()
    {
      if (_shakeCoroutine != null) 
        _coroutineInvoker.StopCoroutine(_shakeCoroutine);
      
      _shakeCoroutine = _coroutineInvoker.StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
      float elapsed = 0f;

      while (elapsed < _cameraShakeConfig.Duration)
      {
        float x = Random.Range(-1f, 1f) * _cameraShakeConfig.Magnitude;
        float y = Random.Range(-1f, 1f) * _cameraShakeConfig.Magnitude;

        _camera.transform.localPosition = _originalPos + new Vector3(x, y, 0f);

        elapsed += Time.deltaTime;
        yield return null;
      }

      _camera.transform.localPosition = _originalPos;
    }
  }
}