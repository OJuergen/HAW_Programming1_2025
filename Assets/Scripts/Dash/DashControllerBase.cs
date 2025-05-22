using UnityEngine;
using UnityEngine.InputSystem;

namespace GameProgramming1
{
    /// <summary>
    /// Base class for dash controllers, handling input and managing state.
    ///
    /// This base class does the following:
    /// - Handle input
    /// - Set <see cref="IsDashing"/>
    /// - Trigger events <see cref="DashStarted"/> and <see cref="DashFinished"/>/>
    /// - Block <see cref="PlayerController"/> from moving while dashing
    /// 
    /// Implementations must do the following:
    /// - Handling dash mechanics
    /// - Call <see cref="FinishDash"/> when the dash is finished
    /// </summary>
    public abstract class DashControllerBase : MonoBehaviour
    {
        [Header("References")]
        [SerializeField, Tooltip("To pause normal movement during dash")]
        private PlayerController _playerController;
        [SerializeField] private InputActionReference _dashAction;

        [Header("Settings")]
        [field: SerializeField, Tooltip("Minimum time in seconds between starting two consecutive dashes.")]
        protected float DashCooldown { get; private set; } = 0.5f;

        [SerializeField, Tooltip("If checked, releasing the input will stop the dash.")]
        private bool _canCancel;

        public bool IsDashing { get; private set; }
        public bool IsCoolingDown { get; protected set; }

        // events
        public delegate void DashEvent(DashControllerBase dashController);

        public event DashEvent DashStarted;
        public event DashEvent DashFinished;

        private void OnEnable()
        {
            _dashAction.action.Enable();
            _dashAction.action.performed += OnDashAction;
            _dashAction.action.canceled += OnDashCanceled;
        }

        private void OnDisable()
        {
            _dashAction.action.performed -= OnDashAction;
            _dashAction.action.canceled -= OnDashCanceled;
        }

        private void OnDashAction(InputAction.CallbackContext callbackContext)
        {
            if (IsCoolingDown) return;

            IsDashing = true;
            _playerController.AddBlocker(this);
            StartDash();
            DashStarted?.Invoke(this);
        }

        private void OnDashCanceled(InputAction.CallbackContext callbackContext)
        {
            if (_canCancel) CancelDash();
        }

        protected abstract void CancelDash();

        protected abstract void StartDash();

        protected void FinishDash()
        {
            if (!IsDashing) return;

            _playerController.RemoveBlocker(this);
            IsDashing = false;
            DashFinished?.Invoke(this);
        }
    }
}