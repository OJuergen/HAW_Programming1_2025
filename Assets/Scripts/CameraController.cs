using UnityEngine;
using UnityEngine.InputSystem;

namespace GameProgramming1
{
    /// <summary>
    /// Simple yaw/pitch camera controller following the player.
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        [Header("Input Actions")]
        [SerializeField] private InputActionReference _zoomAction;
        [SerializeField] private InputActionReference _rotateAction;

        [Header("Settings")]
        [SerializeField, Tooltip("In distance units per input unit.")]
        private float _zoomSpeed = 0.5f;
        [SerializeField, Tooltip("In degrees per input unit.")]
        private float _rotateSpeed = 1f;
        [SerializeField, Tooltip("Zoom is distance between camera and focus point.")]
        private float _defaultZoom = 3f;
        [SerializeField] private float _minZoom = 1f;
        [SerializeField] private float _maxZoom = 10f;
        [SerializeField, Tooltip("Angle in degrees as a function of the zoom level. " +
                                 "Zoom level goes from 0 (closest) to 1 (farthest).")]
        private AnimationCurve _pitchAngleByZoom;

        [Header("State")]
        [SerializeField] private float _zoom;
        [SerializeField] private float _pitchAngle;
        [SerializeField] private float _yawAngle;

        // cache
        private Player _player;

        private void Awake()
        {
            if (_zoomAction == null || _rotateAction == null)
            {
                Debug.LogWarning("No input actions referenced. Disabling.", this);
                enabled = false;
                return;
            }

            _zoom = Mathf.Clamp(_defaultZoom, _minZoom, _maxZoom);
        }

        private void OnEnable()
        {
            _zoomAction.action.Enable();
            _zoomAction.action.performed += OnZoomAction;
            _rotateAction.action.Enable();
            _rotateAction.action.performed += OnRotate;
        }

        private void OnDisable()
        {
            _zoomAction.action.performed -= OnZoomAction;
            _rotateAction.action.performed -= OnRotate;
        }

        private void OnZoomAction(InputAction.CallbackContext context)
        {
            _zoom -= _zoomSpeed * context.ReadValue<Vector2>().y;
            _zoom = Mathf.Clamp(_zoom, _minZoom, _maxZoom);
        }

        private void OnRotate(InputAction.CallbackContext context)
        {
            _yawAngle -= _rotateSpeed * context.ReadValue<Vector2>().x;
        }

        private void Update()
        {
            if (_player == null) _player = FindAnyObjectByType<Player>();
            if (_player == null) return;

            float zoomLevel = Mathf.InverseLerp(_minZoom, _maxZoom, _zoom);
            _pitchAngle = _pitchAngleByZoom.Evaluate(zoomLevel);

            // using spherical coordinates
            Vector3 offset = _zoom * new Vector3(
                Mathf.Cos(Mathf.Deg2Rad * _pitchAngle) * Mathf.Sin(Mathf.Deg2Rad * _yawAngle),
                Mathf.Sin(Mathf.Deg2Rad * _pitchAngle),
                -Mathf.Cos(Mathf.Deg2Rad * _pitchAngle) * Mathf.Cos(Mathf.Deg2Rad * _yawAngle));

            Transform focusPoint = _player.CameraFocusPoint;
            if (focusPoint == null) focusPoint = _player.transform;
            transform.position = focusPoint.position + offset;
            transform.LookAt(focusPoint);

            // special case where camera points downwards vertically needs to be handled differently
            if (Mathf.Approximately(_pitchAngle, 90))
            {
                transform.eulerAngles = new(90, 0, _yawAngle);
            }
        }
    }
}