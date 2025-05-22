using UnityEngine;
using UnityEngine.InputSystem;

namespace GameProgramming1
{
    /// <summary>
    /// Simple, single-component dash controller with some fluff.
    /// We added cooldown, block normal movement during dash, added some helpful events and state.
    /// Next, let's separate the fluff to an abstract base class.
    /// See <see cref="DashControllerBase"/> & <see cref="DashControllerUpdate"/>.
    /// </summary>
    public class DashControllerSimplePlus : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private InputActionReference _dashAction;
        [SerializeField, Tooltip("To pause normal movement during dash")]
        private PlayerController _playerController;

        [Header("Settings")]
        [SerializeField, Tooltip("Time between two starts of dash in seconds.")]
        private float _cooldownTime = 0.5f;
        [SerializeField] private float _dashDistance = 0.5f;
        [SerializeField] private float _dashTime = 0.2f;

        // state
        public bool IsDashing { get; private set; }
        public bool IsCoolingDown { get; protected set; }

        // events
        public delegate void DashEvent(DashControllerSimplePlus dashController);

        public event DashEvent DashStarted;
        public event DashEvent DashFinished;

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
            if (IsCoolingDown) return;
            _dashStartTime = Time.time;
            IsDashing = true;
            IsCoolingDown = true;
            _playerController.AddBlocker(this);
            DashStarted?.Invoke(this);
        }

        private void Update()
        {
            if (IsDashing)
            {
                transform.position += Time.deltaTime / _dashTime * _dashDistance * transform.forward;
                if (Time.time > _dashStartTime + _dashTime)
                {
                    IsDashing = false;
                    _dashStartTime = 0;
                    _playerController.RemoveBlocker(this);
                    DashFinished?.Invoke(this);
                }
            }

            if (IsCoolingDown && Time.time > _dashStartTime + _cooldownTime)
            {
                IsCoolingDown = false;
            }
        }
    }
}