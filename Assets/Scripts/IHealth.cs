namespace GameProgramming1
{
    /// <summary>
    /// Health is responsible to keep track of the current and maximum health of objects that can be damaged.
    /// </summary>
    public interface IHealth
    {
        public int MaxHealth { get; }
        public int CurrentHealth { get; }
        public bool IsAlive => CurrentHealth > 0;

        /// <summary>
        /// Reduce the current health by the given amount.
        /// </summary>
        /// <param name="amount">Damage amount must be positive.</param>
        public void TakeDamage(int amount);

        /// <summary>
        /// Increase the current health by a given amount.
        /// </summary>
        /// <param name="amount">Heal amount must be positive.</param>
        public void Heal(int amount);
    }
}