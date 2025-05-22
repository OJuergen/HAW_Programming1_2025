using UnityEngine;
using UnityEngine.InputSystem;

namespace GameProgramming1
{
    /// <summary>
    /// Most simple, single-component dash controller.
    /// Next, let's add some fluff, see <see cref="DashControllerSimplePlus"/>
    /// </summary>
    public class DashControllerSimple : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private InputActionReference _dashAction;

        [Header("Settings")]
        [SerializeField] private float _dashDistance = 0.5f;
        [SerializeField] private float _dashTime = 0.2f;

        // cache
        private float _dashStartTime;

        private void OnEnable()
        {
            _dashAction.action.Enable();
            _dashAction.action.performed += OnDashAction;
        }

        private void OnDisable()
        {
            _dashAction.action.performed -= OnDashAction;
        }

        private void OnDashAction(InputAction.CallbackContext callbackContext)
        {
            _dashStartTime = Time.time;
        }

        private void Update()
        {
            if (Time.time > _dashStartTime && Time.time < _dashStartTime + _dashTime)
            {
                transform.position += Time.deltaTime / _dashTime * _dashDistance * transform.forward;
            }
        }
    }
}