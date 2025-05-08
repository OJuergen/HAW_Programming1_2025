using System;
using UnityEngine;
using UnityEngine.Events;

namespace GameProgramming1
{
    /// <summary>
    /// An implementation of <see cref="IHealth"/> with Unity events.
    /// Unity events can be serialized and configured in the inspector, so scripts are perfectly decoupled.
    /// You can also add callbacks through code, but remember to keep things consistent and comprehensible.
    /// Unity events come with a small performance impact relative to C# events and less language support.
    /// </summary>
    public class HealthUnityEvents : MonoBehaviour, IHealth
    {
        [field: SerializeField] public int MaxHealth { get; private set; } = 100;
        public int CurrentHealth { get; private set; }
        public bool IsAlive => CurrentHealth > 0;

        [SerializeField] private UnityEvent<(IHealth health, int amount, int newHealth)> _tookDamage;
        [SerializeField] private UnityEvent<(IHealth health, int amount, int newHealth)> _healed;
        [SerializeField] private UnityEvent<(IHealth health, int amount, int newHealth)> _healthChanged;

        private void Awake()
        {
            CurrentHealth = MaxHealth;
            _healthChanged.Invoke((this, MaxHealth, CurrentHealth));
        }

        public void TakeDamage(int amount)
        {
            if (amount < 0) throw new ArgumentException("Cannot take negative damage.");
            if (amount == 0) return;

            CurrentHealth = Mathf.Max(0, CurrentHealth - amount);

            _tookDamage.Invoke((this, amount, CurrentHealth));
            _healthChanged.Invoke((this, amount, CurrentHealth));
        }

        public void Heal(int amount)
        {
            if (amount < 0) throw new ArgumentException("Cannot heal negative amounts.");
            if (amount == 0) return;

            CurrentHealth = Mathf.Min(MaxHealth, CurrentHealth + amount);

            _healed.Invoke((this, amount, CurrentHealth));
            _healthChanged.Invoke((this, amount, CurrentHealth));
        }
    }
}