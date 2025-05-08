using UnityEngine;

namespace GameProgramming1
{
    /// <summary>
    /// As asset-based event.
    /// Can be used to link objects via an asset or as a wrapper for project-scope variables.
    /// An advantage is that assets can be easily referenced. However, this comes at the cost that they can be invoked
    /// from anywhere. We add the sender parameter and the event asset to the event delegate for debuggability.
    ///
    /// While this is comfortable to use, a complex project with asset-based links can become very hard to navigate.
    /// </summary>
    /// <typeparam name="T">Type of event arguments or variable.</typeparam>
    public abstract class EventAsset<T> : ScriptableObject
    {
        public T Value { get; private set; }

        public delegate void EventDelegate(EventAsset<T> eventAsset, object sender, T args);

        public event EventDelegate Invoked;

        public void Invoke(T value, object sender)
        {
            Value = value;
            Invoked?.Invoke(this, sender, value);
        }
    }
}