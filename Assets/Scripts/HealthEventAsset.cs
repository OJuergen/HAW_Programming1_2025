using System;
using UnityEngine;
using UnityEngine.Events;

namespace GameProgramming1
{
    /// <summary>
    /// An implementation of <see cref="IHealth"/> with Asset-based events.
    /// </summary>
    public class HealthAssetEvents : MonoBehaviour, IHealth
    {
        [field: SerializeField] public int MaxHealth { get; private set; } = 100;
        public int CurrentHealth { get; private set; }
        public bool IsAlive => CurrentHealth > 0;

        [SerializeField] private HealthChangeEventAsset _tookDamage;
        [SerializeField] private HealthChangeEventAsset _healed;
        [SerializeField] private HealthChangeEventAsset _healthChanged;

        private void Awake()
        {
            CurrentHealth = MaxHealth;
            _healthChanged.Invoke(this, MaxHealth, CurrentHealth);
        }

        public void TakeDamage(int amount)
        {
            if (amount < 0) throw new ArgumentException("Cannot take negative damage.");
            if (amount == 0) return;

            CurrentHealth = Mathf.Max(0, CurrentHealth - amount);

            if (_tookDamage != null) _tookDamage.Invoke(this, amount, CurrentHealth);
            if (_healthChanged != null) _healthChanged.Invoke(this, amount, CurrentHealth);
        }

        public void Heal(int amount)
        {
            if (amount < 0) throw new ArgumentException("Cannot heal negative amounts.");
            if (amount == 0) return;

            CurrentHealth = Mathf.Min(MaxHealth, CurrentHealth + amount);

            if (_healed != null) _healed.Invoke(this, amount, CurrentHealth);
            if (_healthChanged != null) _healthChanged.Invoke(this, amount, CurrentHealth);
        }
    }
}