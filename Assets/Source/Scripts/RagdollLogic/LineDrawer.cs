using UnityEngine;

namespace Source.Scripts.RagdollLogic
{
  [RequireComponent(typeof(LineRenderer))]
  public class LineDrawer : MonoBehaviour
  {
    [SerializeField] private LineRenderer _lineRenderer;

    private Transform _startTransform;
    private Vector3 _endPoint;

    private void Awake()
    {
      _lineRenderer.positionCount = 2;
      _lineRenderer.widthMultiplier = 0.2f;
      _lineRenderer.useWorldSpace = true;
      _lineRenderer.material.color = Color.green;

      RemoveLine();
    }

    private void OnEnable()
    {
      if (_startTransform) 
        SetPositions(_startTransform.position, _endPoint);
    }

    private void OnDisable()
    {
      SetPositions(Vector3.zero, Vector3.zero);
    }

    private void Update()
    {
      if (!_startTransform)
        return;

      Debug.DrawLine(_startTransform.position, _endPoint, Color.green);
      SetPositions(_startTransform.position, _endPoint);
    }

    public void DrawLine(Transform startTransform, Vector3 endPosition)
    {
      _startTransform = startTransform;
      _endPoint = endPosition;
      enabled = true;
      _lineRenderer.enabled = true;
    }

    public void RemoveLine()
    {
      enabled = false;
      _lineRenderer.enabled = false;
    }

    private void SetPositions(Vector3 startTransformPosition, Vector3 endPoint)
    {
      _lineRenderer.SetPosition(0, startTransformPosition);
      _lineRenderer.SetPosition(1, endPoint);
    }

    private void Reset()
    {
      _lineRenderer = GetComponent<LineRenderer>();
    }
  }
}