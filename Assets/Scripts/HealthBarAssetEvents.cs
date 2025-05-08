using UnityEngine;

namespace GameProgramming1
{
    /// <summary>
    /// Component for connecting a <see cref="GameProgramming1.HealthAssetEvents"/> with a <see cref="HealthBarView"/>.
    /// </summary>
    public class HealthBarAssetEvents : MonoBehaviour
    {
        [SerializeField] private HealthChangeEventAsset _healthChangedEvent;
        [SerializeField] private HealthBarView _view;

        private void Awake()
        {
            if (_healthChangedEvent == null)
            {
                Debug.LogWarning("No health-change event asset references. Disabling", this);
                enabled = false;
            }

            if (_view == null)
            {
                Debug.LogWarning("No view component referenced. Disabling.", this);
            }
        }

        private void OnEnable()
        {
            _healthChangedEvent.Invoked += OnHealthChanged;
        }

        private void OnDisable()
        {
            _healthChangedEvent.Invoked -= OnHealthChanged;
        }

        private void OnHealthChanged(EventAsset<HealthChangeEventAsset.Args> eventAsset, object sender,
            HealthChangeEventAsset.Args args)
        {
            _view.SetPercentage(100f * args.newValue / args.health.MaxHealth);
        }
    }
}