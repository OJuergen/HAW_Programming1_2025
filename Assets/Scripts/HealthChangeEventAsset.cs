using UnityEngine;

namespace GameProgramming1
{
    /// <summary>
    /// Implementation of <see cref="EventAsset{T}"/> for health-change events.
    /// </summary>
    [CreateAssetMenu(menuName = "Game Programming 1/health change asset")]
    public class HealthChangeEventAsset : EventAsset<HealthChangeEventAsset.Args>
    {
        public struct Args
        {
            public IHealth health;
            public int amount;
            public int newValue;
        }

        public void Invoke(IHealth health, int amount, int newValue) =>
            base.Invoke(new Args { health = health, amount = amount, newValue = newValue }, health);
    }
}