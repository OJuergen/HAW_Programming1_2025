using UnityEngine;

namespace GameProgramming1
{
    /// <summary>
    /// Component for connecting a <see cref="HealthCSharpEvents"/> with a <see cref="HealthBarView"/>.
    /// </summary>
    public class HealthBarCSharpEvents : MonoBehaviour
    {
        [SerializeField]
        private HealthCSharpEvents _health; // we cannot use the interface type here. It is not serializable
        [SerializeField] private HealthBarView _view;

        private void Awake()
        {
            if (_health == null)
            {
                Debug.LogWarning("No health script referenced for health bar. Disabling", this);
                enabled = false;
            }

            if (_view == null)
            {
                Debug.LogWarning("No view component referenced. Disabling.", this);
            }
        }

        private void OnEnable()
        {
            _health.HealthChanged += OnHealthChanged;
        }

        private void OnDisable()
        {
            _health.HealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged(int newHealth)
        {
            _view.SetPercentage(100f * newHealth / _health.MaxHealth);
        }
    }
}