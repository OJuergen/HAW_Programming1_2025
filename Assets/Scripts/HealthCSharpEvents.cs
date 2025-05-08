using System;
using UnityEngine;

namespace GameProgramming1
{
    /// <summary>
    /// An implementation of <see cref="IHealth"/> with C# events.
    /// C# events are first-class citizens of C# offering a lot of language/IDE support.
    /// Only the defining script can invoke the events & you can name parameters with delegates. 
    /// </summary>
    public class HealthCSharpEvents : MonoBehaviour, IHealth
    {
        [field: SerializeField] public int MaxHealth { get; private set; } = 100;
        public int CurrentHealth { get; private set; }
        public bool IsAlive => CurrentHealth > 0;

        public delegate void HealthChange(HealthCSharpEvents healthCSharpEvents, int amount, int newHealth);

        public event HealthChange TookDamage;
        public event HealthChange Healed;
        public event Action<int> HealthChanged;

        private void Awake()
        {
            CurrentHealth = MaxHealth;
            HealthChanged?.Invoke(CurrentHealth);
        }

        public void TakeDamage(int amount)
        {
            if (amount < 0) throw new ArgumentException("Cannot take negative damage.");
            if (amount == 0) return;

            CurrentHealth = Mathf.Max(0, CurrentHealth - amount);

            TookDamage?.Invoke(this, amount, CurrentHealth);
            HealthChanged?.Invoke(CurrentHealth);
        }

        public void Heal(int amount)
        {
            if (amount < 0) throw new ArgumentException("Cannot heal negative amounts.");
            if (amount == 0) return;

            CurrentHealth = Mathf.Min(MaxHealth, CurrentHealth + amount);

            Healed?.Invoke(this, amount, CurrentHealth);
            HealthChanged?.Invoke(CurrentHealth);
        }
    }
}